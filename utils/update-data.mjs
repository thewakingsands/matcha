import { mkdir, readFile, writeFile } from 'node:fs/promises'
import { join, resolve } from 'node:path'
import { fileURLToPath, pathToFileURL } from 'node:url'
import { listItems, requestJson, xivapiLanguages } from './lib/xivapi-v2.mjs'

const outputDir = fileURLToPath(
  new URL('../Cafe.Matcha/data/', import.meta.url),
)
const locationsFile = new URL('./cache/fate-locations.json', import.meta.url)
const fatesFile = new URL('./data/fates.json', import.meta.url)
const serverUrl = 'https://zhyupe.github.io/ffxiv-datamining-worker/server.json'
const localized = (column) =>
  xivapiLanguages.map((lang) => `${column}@lang(${lang})`)
const cleanString = (value = '') =>
  value
    .replace(/<[^>]*>/g, '')
    .replace(/[\xa0\xad]/g, '')
    .replace(/[\r\n]+/g, ' ')
const formatStrings = (fields, column = 'Name') =>
  Object.fromEntries(
    xivapiLanguages.map((lang) => [
      lang,
      cleanString(fields[`${column}@lang(${lang})`]),
    ]),
  )
const hasName = (row) =>
  Boolean(row.fields['Name@lang(en)'] || row.fields['Name@lang(chs)'])
const dictionary = (rows, transform) =>
  Object.fromEntries(
    rows.map((row) => [row.row_id, transform(row.fields, row)]),
  )

export async function loadFateLocations({
  file = locationsFile,
  fallbackFile = fatesFile,
} = {}) {
  let data
  try {
    data = JSON.parse(await readFile(file, 'utf8'))
  } catch (error) {
    if (error.code !== 'ENOENT') throw error
    const mapping = JSON.parse(await readFile(fallbackFile, 'utf8'))
    if (
      !mapping ||
      Array.isArray(mapping) ||
      typeof mapping !== 'object' ||
      !Object.keys(mapping).length ||
      Object.entries(mapping).some(
        ([id, location]) =>
          !/^[1-9]\d*$/.test(id) ||
          !location ||
          !Number.isSafeInteger(location.territoryId) ||
          location.territoryId <= 0 ||
          !Number.isSafeInteger(location.patch) ||
          location.patch < 2,
      )
    ) {
      throw new Error('Invalid simplified FATE locations')
    }
    return mapping
  }
  if (!Array.isArray(data?.locations) || !data.locations.length) {
    throw new Error('Invalid raw FATE locations')
  }
  const mapping = {}
  for (const { fateId, territoryId, lgbPath } of data.locations) {
    if (
      !Number.isSafeInteger(fateId) ||
      fateId <= 0 ||
      !Number.isSafeInteger(territoryId) ||
      territoryId <= 0
    ) {
      throw new Error('Invalid raw FATE location IDs')
    }
    const expansion = /^bg\/ex(\d+)\//.exec(lgbPath)
    const patch = /^bg\/ffxiv\//.test(lgbPath)
      ? 2
      : expansion
        ? Number(expansion[1]) + 2
        : undefined
    if (!Number.isSafeInteger(patch)) {
      throw new Error(
        `FATE ${fateId}: cannot resolve version from path ${lgbPath}`,
      )
    }
    // Shared LGB files produce multiple territories. Keep the first source entry.
    mapping[fateId] ??= { territoryId, patch }
  }
  return mapping
}

export async function buildData(
  fateLocations,
  blacklist,
  cnServers,
  readSheet = listItems,
) {
  // Fetch everything before writing data. Sequential sheets avoid flooding XIVAPI.
  const sheets = {}
  for (const [sheet, fields] of Object.entries({
    ContentFinderCondition: [
      ...localized('Name'),
      'ContentType.value',
      'ContentMemberType.value',
      'ClassJobLevelRequired',
      'ClassJobLevelSync',
      'ItemLevelRequired',
      'ItemLevelSync',
    ],
    ContentType: localized('Name'),
    ContentRoulette: localized('Name'),
    TerritoryType: localized('PlaceName.Name'),
    ExVersion: localized('Name'),
    RelicNote: [...localized('EventItem.Singular'), 'Fate[].value'],
    Fate: [...localized('Name'), 'ClassJobLevel'],
    DynamicEventSet: localized('DynamicEvent.Name'),
    World: [
      'Name',
      'IsPublic',
      'DataCenter.Name',
      'DataCenter.Region.value',
      'DataCenter.IsCloud',
    ],
  })) {
    sheets[sheet] = await readSheet(sheet, fields)
    if (!sheets[sheet].length) throw new Error(`Empty XIVAPI sheet: ${sheet}`)
    console.log(`${sheet}: ${sheets[sheet].length} rows`)
  }

  const territories = new Map(
    sheets.TerritoryType.map((row) => [row.row_id, row.fields]),
  )
  const patches = dictionary(sheets.ExVersion, (fields) =>
    formatStrings(fields),
  )
  const fateBlacklist = new Set(blacklist)
  let unmapped = 0
  const fates = dictionary(
    sheets.Fate.filter(
      (row) =>
        hasName(row) &&
        !row.fields['Name@lang(en)']?.startsWith('※') &&
        !fateBlacklist.has(cleanString(row.fields['Name@lang(chs)'])),
    ),
    (fields, row) => {
      const { territoryId, patch } = fateLocations[row.row_id] ?? {}
      const territory = territories.get(territoryId)
      const known =
        territoryId && territory?.PlaceName.value && patches[patch - 2]
      if (territoryId && !known)
        throw new Error(
          `FATE ${row.row_id}: cannot resolve territory ${territoryId} or patch ${patch}`,
        )
      if (!known) unmapped++
      return {
        name: formatStrings(fields),
        level: fields.ClassJobLevel,
        patch: known ? patch : 0,
        location: known ? territoryId : 0,
      }
    },
  )
  console.log(
    `FATEs without a local location: ${unmapped} (location/patch = 0)`,
  )

  // DynamicEventSet has no TerritoryType link. Keep the game's existing group mapping.
  // Subrow IDs are event IDs on the wire, and must not be collapsed into row IDs.
  const eventTerritories = { 1: 920, 2: 975, 3: 1252 }
  const dynamicEvents = {}
  const skippedGroups = new Set()
  for (const row of sheets.DynamicEventSet) {
    if (!row.fields.DynamicEvent.value) continue
    const territoryId = eventTerritories[row.row_id]
    if (!territoryId) {
      skippedGroups.add(row.row_id)
      continue
    }
    dynamicEvents[territoryId * 1000 + row.subrow_id] = {
      name: formatStrings(row.fields.DynamicEvent.fields),
    }
  }
  if (skippedGroups.size)
    console.warn(
      `Unmapped DynamicEventSet groups skipped: ${[...skippedGroups].join(', ')}`,
    )

  const worlds = dictionary(
    sheets.World.filter(
      ({ fields }) =>
        fields.Name &&
        (fields.IsPublic || fields.DataCenter.fields.Region?.value === 6) &&
        !fields.DataCenter.fields.IsCloud &&
        fields.DataCenter.fields.Name,
    ),
    (fields) => {
      let dc = fields.DataCenter.fields.Name
      if (dc === 'Eorzea') {
        dc = 'Korea'
      }

      return {
        name: fields.Name,
        name_en: fields.Name,
        dc,
        dc_en: dc,
      }
    },
  )
  if (!Array.isArray(cnServers) || !cnServers.length)
    throw new Error('Invalid Chinese server data')
  for (const server of cnServers) {
    if (
      !server.name_chs ||
      !server.name_en ||
      !Array.isArray(server.worlds) ||
      !server.worlds.length
    ) {
      throw new Error('Invalid Chinese data center')
    }
    for (const world of server.worlds) {
      if (!Number.isInteger(world.id) || !world.name_chs || !world.name_en)
        throw new Error('Invalid Chinese world')
      worlds[world.id] = {
        name: world.name_chs,
        name_en: world.name_en,
        dc: server.name_chs,
        dc_en: server.name_en,
      }
    }
  }

  return {
    'instance.json': dictionary(
      sheets.ContentFinderCondition.filter(hasName),
      (fields) => ({
        name: formatStrings(fields),
        type: fields.ContentType.value,
        level: fields.ClassJobLevelRequired,
        levelSync: fields.ClassJobLevelSync,
        item: fields.ItemLevelRequired,
        itemSync: fields.ItemLevelSync,
        memberType: fields.ContentMemberType.value,
      }),
    ),
    'type.json': dictionary(sheets.ContentType.filter(hasName), (fields) =>
      formatStrings(fields),
    ),
    'roulette.json': dictionary(
      sheets.ContentRoulette.filter(hasName),
      (fields) => formatStrings(fields),
    ),
    'territory.json': dictionary(
      sheets.TerritoryType.filter((row) => row.fields.PlaceName.value),
      (fields) => formatStrings(fields.PlaceName.fields),
    ),
    'patch.json': Object.fromEntries(
      Object.entries(patches).map(([id, name]) => [Number(id) + 2, name]),
    ),
    'template.json': sheets.RelicNote.filter(
      (row) => row.fields.EventItem.value,
    ).map(({ fields }) => ({
      name: formatStrings(fields.EventItem.fields, 'Singular'),
      fate: fields.Fate.map((fate) => fate.value),
    })),
    'fate.json': fates,
    'dynamic-event.json': dynamicEvents,
    'world.json': worlds,
  }
}

async function main() {
  const args = process.argv.slice(2)
  if (args.length) throw new Error('Usage: node utils/update-data.mjs')
  const fateLocations = await loadFateLocations()
  const cnServers = await requestJson(serverUrl)
  const blacklist = JSON.parse(
    await readFile(
      new URL('./data/fate-blacklist.data.json', import.meta.url),
      'utf8',
    ),
  )
  const data = await buildData(fateLocations, blacklist, cnServers)
  await mkdir(outputDir, { recursive: true })
  for (const [file, content] of Object.entries(data)) {
    if (!Object.keys(content).length)
      throw new Error(`Refusing to write empty ${file}`)
  }
  for (const [file, content] of Object.entries(data)) {
    await writeFile(join(outputDir, file), JSON.stringify(content, null, 2))
    console.log(`Updated ${file}: ${Object.keys(content).length} entries`)
  }
  await writeFile(fatesFile, JSON.stringify(fateLocations, null, 2))
  console.log(
    `Updated utils/data/fates.json: ${Object.keys(fateLocations).length} entries`,
  )
}

if (
  process.argv[1] &&
  import.meta.url === pathToFileURL(resolve(process.argv[1])).href
) {
  await main()
}

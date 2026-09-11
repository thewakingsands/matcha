import assert from 'node:assert/strict'
import { mkdtemp, rm, writeFile } from 'node:fs/promises'
import { tmpdir } from 'node:os'
import { join } from 'node:path'
import { test } from 'node:test'
import { pathToFileURL } from 'node:url'
import { listItems, requestJson } from './lib/xivapi-v2.mjs'
import { buildData, loadFateLocations, loadDynamicEventLocations } from './update-data.mjs'

test('pagination retains subrow zero and merges languages with different row sets', async (t) => {
  const requests = []
  t.mock.method(globalThis, 'fetch', async (url) => {
    const query = new URL(url).searchParams
    requests.push(query)
    const rows =
      query.get('language') === 'tc'
        ? [{ row_id: 1, subrow_id: 0, fields: { 'Name@lang(tc)': '舊資料' } }]
        : query.has('after')
          ? [
              {
                row_id: 100,
                subrow_id: 1,
                fields: { 'Name@lang(en)': 'Second' },
              },
            ]
          : Array.from({ length: 100 }, (_, i) => ({
              row_id: i + 1,
              subrow_id: 0,
              fields: { 'Name@lang(en)': `Row ${i + 1}` },
            }))
    return Response.json({ rows })
  })
  const rows = await listItems('Example', ['Name@lang(en)', 'Name@lang(tc)'])
  assert.equal(requests[1].get('after'), '100:0')
  assert.equal(rows.length, 101)
  assert.deepEqual(rows[0].fields, {
    'Name@lang(en)': 'Row 1',
    'Name@lang(tc)': '舊資料',
  })
  assert.equal(rows.at(-1).subrow_id, 1)
})

test('HTTP failures and malformed sheet responses fail instead of returning partial data', async (t) => {
  const mock = t.mock.method(
    globalThis,
    'fetch',
    async () => new Response('', { status: 404 }),
  )
  await assert.rejects(requestJson('https://example.test'), /HTTP 404/)
  assert.equal(mock.mock.callCount(), 1)
  mock.mock.mockImplementation(async () =>
    Response.json({ message: 'bad sheet' }),
  )
  await assert.rejects(listItems('Example', ['Name']), /bad sheet/)
})

test('raw locations take precedence and missing raw data uses the simplified map without fetching', async (t) => {
  const dir = await mkdtemp(join(tmpdir(), 'matcha-data-'))
  t.after(() => rm(dir, { recursive: true, force: true }))
  const file = pathToFileURL(join(dir, 'fate-locations.json'))
  const fallbackFile = pathToFileURL(join(dir, 'fates.json'))
  const mock = t.mock.method(globalThis, 'fetch', () => {
    throw new Error('Unexpected network request')
  })
  const fallback = { 120: { territoryId: 148, patch: 2 } }
  await writeFile(fallbackFile, JSON.stringify(fallback))
  assert.deepEqual(await loadFateLocations({ file, fallbackFile }), fallback)
  await writeFile(
    file,
    JSON.stringify({
      locations: [
        {
          fateId: 120,
          territoryId: 153,
          lgbPath: 'bg/ffxiv/fst/level/planevent.lgb',
        },
        {
          fateId: 120,
          territoryId: 190,
          lgbPath: 'bg/ex1/fst/level/planevent.lgb',
        },
        {
          fateId: 121,
          territoryId: 148,
          lgbPath: 'bg/ex3/fst/level/planevent.lgb',
        },
        {
          fateId: 122,
          territoryId: 149,
          lgbPath: 'bg/ex5/fst/level/planevent.lgb',
        },
      ],
    }),
  )
  assert.deepEqual(await loadFateLocations({ file, fallbackFile }), {
    120: { territoryId: 153, patch: 2 },
    121: { territoryId: 148, patch: 5 },
    122: { territoryId: 149, patch: 7 },
  })
  await writeFile(
    file,
    JSON.stringify({
      locations: [
        { fateId: 120, territoryId: 153, lgbPath: 'bg/unknown/planevent.lgb' },
      ],
    }),
  )
  await assert.rejects(
    loadFateLocations({ file, fallbackFile }),
    /cannot resolve version/,
  )
  await writeFile(file, JSON.stringify({ locations: [] }))
  await assert.rejects(loadFateLocations({ file, fallbackFile }), /Invalid raw/)
  await writeFile(file, '{invalid')
  await assert.rejects(loadFateLocations({ file, fallbackFile }), SyntaxError)
  await rm(file)
  await writeFile(fallbackFile, JSON.stringify({ 120: '148' }))
  await assert.rejects(
    loadFateLocations({ file, fallbackFile }),
    /Invalid simplified/,
  )
  await writeFile(fallbackFile, JSON.stringify({ 120: { territoryId: 148 } }))
  await assert.rejects(
    loadFateLocations({ file, fallbackFile }),
    /Invalid simplified/,
  )
  await rm(fallbackFile)
  await assert.rejects(loadFateLocations({ file, fallbackFile }), {
    code: 'ENOENT',
  })
  assert.equal(mock.mock.callCount(), 0)
})

test('dynamic event locations prefer raw records and fall back only when the source is absent', async (t) => {
  const dir = await mkdtemp(join(tmpdir(), 'matcha-events-'))
  t.after(() => rm(dir, { recursive: true, force: true }))
  const file = pathToFileURL(join(dir, 'locations.json'))
  const fallbackFile = pathToFileURL(join(dir, 'dynamic-events.json'))
  await writeFile(fallbackFile, JSON.stringify({ 49: 1346 }))
  assert.deepEqual(await loadDynamicEventLocations({ file, fallbackFile }), { 49: 1346 })
  await writeFile(file, JSON.stringify({ dynamicEventLocations: [
    { dynamicEventId: 49, territoryId: 148 },
    { dynamicEventId: 49, territoryId: 190 },
    { dynamicEventId: 50, territoryId: 153 },
  ] }))
  assert.deepEqual(await loadDynamicEventLocations({ file, fallbackFile }), { 49: 148, 50: 153 })
  await writeFile(file, JSON.stringify({ locations: [] }))
  await assert.rejects(loadDynamicEventLocations({ file, fallbackFile }), /Invalid raw/)
  await writeFile(file, JSON.stringify({ dynamicEventLocations: [{ dynamicEventId: 1, territoryId: 0 }] }))
  await assert.rejects(loadDynamicEventLocations({ file, fallbackFile }), /Invalid raw/)
  await writeFile(file, '{invalid')
  await assert.rejects(loadDynamicEventLocations({ file, fallbackFile }), SyntaxError)
  await rm(file)
  await writeFile(fallbackFile, JSON.stringify({ 49: '1346' }))
  await assert.rejects(loadDynamicEventLocations({ file, fallbackFile }), /Invalid simplified/)
  await rm(fallbackFile)
  await assert.rejects(loadDynamicEventLocations({ file, fallbackFile }), { code: 'ENOENT' })
})

test('all nine outputs preserve IDs, expansions, templates, filters and CN server overrides', async () => {
  const name = (value) => ({ 'Name@lang(en)': value })
  const row = (row_id, fields, subrow_id) => ({ row_id, fields, subrow_id })
  const sheets = {
    ContentFinderCondition: [
      row(1, {
        ...name('New duty'),
        ContentType: { value: 2 },
        ContentMemberType: { value: 1 },
        ClassJobLevelRequired: 90,
        ClassJobLevelSync: 100,
        ItemLevelRequired: 600,
        ItemLevelSync: 700,
      }),
    ],
    ContentType: [row(2, name('Dungeon'))],
    ContentRoulette: [row(1, name('Roulette'))],
    TerritoryType: [
      row(148, {
        PlaceName: { value: 54, fields: name('Forest') },
        ExVersion: { value: 0 },
      }),
      row(1346, { PlaceName: { value: 5577, fields: name('North Horn') } }),
    ],
    ExVersion: [row(0, name('A Realm Reborn')), row(3, name('Shadowbringers'))],
    RelicNote: [
      row(1, {
        EventItem: { value: 10, fields: { 'Singular@lang(en)': 'Book' } },
        Fate: [{ value: 120 }, { value: 121 }, { value: 122 }],
      }),
    ],
    Fate: [
      row(120, { ...name('Fate\nname'), ClassJobLevel: 5 }),
      row(121, { ...name('Unknown'), ClassJobLevel: 80 }),
      row(122, { ...name('Blocked'), 'Name@lang(chs)': '黑名单' }),
      row(123, name('※Unused')),
    ],
    DynamicEventSet: [
      row(1, { DynamicEvent: { value: 16, fields: name('Siege') } }, 0),
      row(1, { DynamicEvent: { value: 1, fields: name('Engagement') } }, 1),
      row(4, { DynamicEvent: { value: 64, fields: name('Tower') } }, 0),
      row(4, { DynamicEvent: { value: 49, fields: name('North event') } }, 1),
      row(5, { DynamicEvent: { value: 65, fields: name('Alternate tower') } }, 0),
      row(5, { DynamicEvent: { value: 49, fields: name('North event') } }, 1),
      row(6, { DynamicEvent: { value: 70, fields: name('No location') } }, 0),
      row(7, { DynamicEvent: { value: 71, fields: name('Ambiguous group') } }, 0),
      row(7, { DynamicEvent: { value: 1, fields: name('Engagement') } }, 2),
      row(7, { DynamicEvent: { value: 49, fields: name('North event') } }, 2),
    ],
    World: [
      row(21, {
        Name: 'Ravana',
        IsPublic: true,
        DataCenter: { fields: { Name: 'Materia' } },
      }),
      row(1042, {
        Name: 'API name',
        IsPublic: true,
        DataCenter: { fields: { Name: 'API DC' } },
      }),
      row(99, { Name: 'Test', IsPublic: false, DataCenter: { fields: {} } }),
      row(2075, {
        Name: '카벙클',
        IsPublic: false,
        DataCenter: { fields: { Name: 'Korea', Region: { value: 6 } } },
      }),
      row(3000, {
        Name: 'Cloudtest01',
        IsPublic: true,
        DataCenter: { fields: { Name: 'Cloud', IsCloud: true } },
      }),
    ],
  }
  const data = await buildData(
    { 120: { territoryId: 148, patch: 5 } },
    { 1: 148, 49: 1346 },
    ['黑名单'],
    [
      {
        name_chs: '陆行鸟',
        name_en: 'LuXingNiao',
        worlds: [{ id: 1042, name_chs: '拉诺西亚', name_en: 'LaNuoXiYa' }],
      },
    ],
    async (sheet) => sheets[sheet],
  )
  assert.equal(Object.keys(data).length, 9)
  assert.equal(data['instance.json'][1].name.chs, '')
  assert.equal(data['instance.json'][1].itemSync, 700)
  // Use the path-derived patch, even when TerritoryType.ExVersion differs.
  assert.equal(data['fate.json'][120].patch, 5)
  assert.equal(data['fate.json'][120].location, 148)
  assert.equal(data['fate.json'][120].name.en, 'Fate name')
  assert.equal(data['fate.json'][121].patch, 0)
  assert.equal(data['fate.json'][121].location, 0)
  assert.deepEqual(Object.keys(data['fate.json']), ['120', '121'])
  assert.deepEqual(data['template.json'][0].fate, [120, 121, 122])
  assert.deepEqual(Object.keys(data['dynamic-event.json']), [
    '148000',
    '148001',
    '148002',
    '1346000',
    '1346001',
    '1346002',
  ])
  assert.equal(data['dynamic-event.json'][148000].name.en, 'Siege')
  assert.equal(data['dynamic-event.json'][1346000].name.en, 'Tower')
  assert.equal(data['world.json'][1042].dc_en, 'LuXingNiao')
  assert.equal(data['world.json'][21].name_en, 'Ravana')
  assert.equal(data['world.json'][99], undefined)
  assert.equal(data['world.json'][2075].dc, 'Korea')
  assert.equal(data['world.json'][3000], undefined)
})

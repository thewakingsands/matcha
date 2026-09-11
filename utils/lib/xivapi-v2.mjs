import { setTimeout } from 'node:timers/promises'

export const xivapiLanguages = ['chs', 'en', 'ja', 'de', 'fr']
// export const xivapiLanguages = ['chs', 'tc', 'ko', 'en', 'ja', 'de', 'fr']

const xivapiV2 = 'https://xivapi-v2.xivcdn.com/'
const limit = 100
export async function requestJson(url) {
  for (let attempt = 0; ; attempt++) {
    try {
      const res = await fetch(url, { signal: AbortSignal.timeout(60_000) })
      if (!res.ok) {
        const error = new Error(`HTTP ${res.status}: ${url}`)
        error.retryable = res.status === 429 || res.status >= 500
        throw error
      }
      return await res.json()
    } catch (error) {
      if (error.retryable === false || attempt >= 3) throw error
      await setTimeout(1000 * 2 ** attempt)
    }
  }
}

async function listLanguageItems(sheet, fields, language) {
  const search = new URLSearchParams()
  search.set('fields', fields.join(','))
  search.set('limit', limit)
  search.set('language', language)

  const result = []
  while (true) {
    const data = await requestJson(`${xivapiV2}api/sheet/${sheet}?${search}`)
    if (!Array.isArray(data.rows))
      throw new Error(`xivapi-v2 ${sheet}: ${data.message || 'missing rows'}`)
    result.push(...data.rows)
    if (data.rows.length < limit) break
    const last = data.rows.at(-1)
    // Preserve subrow zero when a page ends at the start of a group.
    const after =
      last.subrow_id == null
        ? `${last.row_id}`
        : `${last.row_id}:${last.subrow_id}`
    if (after === search.get('after'))
      throw new Error(`xivapi-v2 ${sheet}: pagination did not advance`)
    search.set('after', after)
  }

  return result
}

function mergeFields(target, source) {
  for (const [key, value] of Object.entries(source)) {
    if (value && typeof value === 'object') {
      target[key] ??= Array.isArray(value) ? [] : {}
      mergeFields(target[key], value)
    } else {
      target[key] = value
    }
  }
}

export async function listItems(sheet, fields) {
  // Enumerate each language's own rows: regional versions may lack newer rows.
  const groups = new Map()
  for (const field of fields) {
    const language = field.match(/@lang\((\w+)\)/)?.[1] || 'en'
    if (!groups.has(language)) groups.set(language, [])
    groups.get(language).push(field)
  }
  const rows = new Map()
  for (const [language, languageFields] of groups) {
    for (const row of await listLanguageItems(
      sheet,
      languageFields,
      language,
    )) {
      const key = `${row.row_id}:${row.subrow_id ?? ''}`
      if (!rows.has(key)) rows.set(key, { ...row, fields: {} })
      mergeFields(rows.get(key).fields, row.fields)
    }
  }
  return [...rows.values()].sort(
    (a, b) => a.row_id - b.row_id || (a.subrow_id ?? 0) - (b.subrow_id ?? 0),
  )
}

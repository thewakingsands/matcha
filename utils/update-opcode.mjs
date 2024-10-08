import { readCsv } from './csv.mjs'
import { formatOpcode, parseOpcode } from './opcode.mjs'
import fetch from 'node-fetch'
import { join } from 'path'
import { fileURLToPath } from 'url'
import { writeFileSync } from 'fs'

const __dirname = fileURLToPath(new URL('.', import.meta.url))

const opcodes = [
  'ActorControl',
  'ActorControlSelf',
  {
    // https://github.com/quisquous/cactbot/blob/main/plugin/CactbotEventSource/FateWatcher.cs#L45
    key: 'CEDirector',
    // this is not the actual key, just a hack
    karashiiro: '_GH_CEDirector'
  },
  {
    key: 'CompanyAirshipStatus',
    karashiiro: 'AirshipTimers'
  },
  {
    key: 'CompanySubmersibleStatus',
    karashiiro: 'SubmarineTimers'
  },
  {
    key: 'ContentFinderNotifyPop',
    karashiiro: 'CFNotify'
  },
  {
    key: 'DirectorStart'
    // karashiiro: 'MiniCactpotInit'
  },
  'EventPlay',
  'Examine',
  'FateInfo',
  'InitZone',
  'InventoryTransaction',
  'ItemInfo',
  'MarketBoardItemListing',
  'MarketBoardItemListingCount',
  'MarketBoardItemListingHistory',
  'MarketBoardRequestItemListingInfo',
  'NpcSpawn',
  'PlayerSetup',
  'PlayerSpawn'
]

const outputOpcode = (key, value) =>
  `${' '.repeat(12)}{ 0x${formatOpcode(value)}, MatchaOpcode.${key} },`

const outputKeys = () =>
  opcodes
    .map((item, index) => {
      if (typeof item === 'string') {
        item = { key: item }
      }

      return `${' '.repeat(8)}${item.key},`
    })
    .join('\n')

const outputFromKarashiiro = (list, region) =>
  opcodes
    .map((item, index) => {
      if (typeof item === 'string') {
        item = { key: item }
      }

      const { key } = item
      if (item[region]) {
        return outputOpcode(key, item[region])
      }

      const fromKey = item.karashiiro || item.key
      const row = list.lists.ServerZoneIpcType.find(
        (row) => row.name === fromKey
      )
      const value = row ? row.opcode : 0xf000 + index

      return outputOpcode(key, value)
    })
    .join('\n')

const outputFromWorker = (list) =>
  opcodes
    .map((item, index) => {
      if (typeof item === 'string') {
        item = { key: item }
      }
      const { key } = item
      const row = list.find(([rowKey]) => rowKey === key)
      const value = row ? row[1] : 0xf000 + index

      return outputOpcode(key, value)
    })
    .join('\n')

  ; (async () => {
    const karashiiroData = await fetch(
      'https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/opcodes.json'
    )
    const parsedData = await karashiiroData.json()

    const globalOpcodes = parsedData.find((item) => item.region === 'Global')

    const cactbotFate = await fetch(
      'https://raw.githubusercontent.com/quisquous/cactbot/main/plugin/CactbotEventSource/FateWatcher.cs'
    )
    const ceDirector =
      /cedirector_intl.+\n.+0x30.+\n\s+(0x[0-9a-fA-F]+),?\s*\n\s*\)/.exec(
        await cactbotFate.text()
      )

    if (ceDirector) {
      globalOpcodes.lists.ServerZoneIpcType.push({
        name: '_GH_CEDirector',
        opcode: parseOpcode(ceDirector[1])
      })
    }

    const workerData = await fetch(
      'https://raw.githubusercontent.com/zhyupe/ffxiv-opcode-worker/master/cn-opcodes.csv'
    )
    const workerLines = readCsv(await workerData.text(), null, {
      header: 0,
      skip: 0
    })
    const cnOpcodes = workerLines.map(({ Name: name, Scope: scope, _ }) => {
      const valueColumn = _.reduce((val, content, index) => {
        return content ? index : val
      }, 0)

      const isClient = scope === 'ClientZoneIpc'
      return [name, (isClient ? 0x8000 : 0) + parseOpcode(_[valueColumn])]
    })

    writeFileSync(
      join(__dirname, '../Cafe.Matcha/Constant/MatchaOpcode.cs'),
      `// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Constant
{
    using System.Collections.Generic;

    internal enum MatchaOpcode
    {
${outputKeys()}
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, MatchaOpcode> Global = new Dictionary<ushort, MatchaOpcode>
        {
${outputFromKarashiiro(globalOpcodes, 'global')}
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
${outputFromWorker(cnOpcodes, 'cn')}
        };
    }
}
`
    )
  })()

import { writeFileSync } from 'fs'
import { join } from 'path'
import { fileURLToPath } from 'url'
import { formatOpcode, parseOpcode } from './lib/opcode.mjs'

const __dirname = fileURLToPath(new URL('.', import.meta.url))

const opcodes = [
  'ActorControl',
  'ActorControlSelf',
  'CEDirector',
  'CompanyAirshipStatus',
  'CompanySubmersibleStatus',
  'ContentFinderNotifyPop',
  'ResumeEventScene32',
  'EventPlay',
  'EventStart',
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
  'PlayerSpawn',
  'SubmarineStatusList',
  'WorldVisitQueue',
  'EventPlay4',
  'SystemLogMessage',
  'FishCaught',
  'StatusEffectList',
  'ClientTrigger',
]
const clientOpcodes = new Set(['MarketBoardRequestItemListingInfo', 'ClientTrigger'])

const outputOpcode = (key, value) =>
  `${' '.repeat(12)}{ 0x${formatOpcode(value)}, MatchaOpcode.${key} },`

const outputKeys = () =>
  opcodes.map((key) => `${' '.repeat(8)}${key},`).join('\n')

const outputFromWorker = (list) =>
  opcodes
    .map((key) => {
      const value = parseOpcode(list[key])
      if (!Number.isInteger(value) || value < 0 || value > 0x7fff) {
        throw new Error(`Missing or invalid opcode: ${key}`)
      }
      return outputOpcode(key, value | (clientOpcodes.has(key) ? 0x8000 : 0))
    })
    .join('\n')

;(async () => {
  const workerData = await fetch(
    'https://raw.githubusercontent.com/zhyupe/ffxiv-opcode-worker/master/json/current.json',
  )
  if (!workerData.ok) {
    throw new Error(`Cannot fetch CN opcodes: HTTP ${workerData.status}`)
  }
  const cnOpcodes = await workerData.json()

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
${outputFromWorker(cnOpcodes)}
        };
        public static Dictionary<ushort, MatchaOpcode> China = new Dictionary<ushort, MatchaOpcode>
        {
${outputFromWorker(cnOpcodes)}
        };
    }
}
`,
  )
})()

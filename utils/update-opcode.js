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
    key: 'DirectorStart',
    global: 0x256
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
  'NpcSpawn',
  'PlayerSetup',
  'PlayerSpawn'
]

const https = require('https')
const fs = require('fs')
const { join } = require('path')

const request = (url) => new Promise((resolve, reject) => {
  const req = https.request(url, (res) => {
    const chunks = []
    res.on('data', (chunk) => chunks.push(chunk))
    res.on('end', () => resolve(Buffer.concat(chunks)))
  })
  req.on('error', reject)
  req.end()
})

const outputOpcode = (key, value) => `${' '.repeat(12)}{ 0x${value.toString(16).padStart(4, '0')}, MatchaOpcode.${key} },`

const outputKeys = () => opcodes.map((item, index) => {
  if (typeof item === 'string') {
    item = { key: item }
  }

  return `${' '.repeat(8)}${item.key},`
}).join('\n')

const outputFromKarashiiro = (list, region) => opcodes.map((item, index) => {
  if (typeof item === 'string') {
    item = { key: item }
  }

  const { key } = item
  if (item[region]) {
    return outputOpcode(key, item[region])
  }

  const fromKey = item.karashiiro || item.key
  const row = list.lists.ServerZoneIpcType.find((row) => row.name === fromKey)
  const value = row ? row.opcode : (0xF000 + index)

  return outputOpcode(key, value)
}).join('\n')

const outputFromWorker = (list) => opcodes.map((item, index) => {
  if (typeof item === 'string') {
    item = { key: item }
  }
  const { key } = item
  const row = list.find(([rowKey]) => rowKey === key)
  const value = row ? row[1] : (0xF000 + index)

  return outputOpcode(key, value)
}).join('\n')

;(async () => {
  const karashiiroData = await request('https://raw.githubusercontent.com/karashiiro/FFXIVOpcodes/master/opcodes.json')
  const parsedData = JSON.parse(karashiiroData.toString())

  const globalOpcodes = parsedData.find(item => item.region === 'Global')

  const cactbotFate = await request('https://raw.githubusercontent.com/quisquous/cactbot/main/plugin/CactbotEventSource/FateWatcher.cs')
  const ceDirector = /cedirector_intl.+\n.+0x30.+\n\s+(0x[0-9a-fA-F]+),?\s*\n\s*\)/.exec(cactbotFate.toString())

  if (ceDirector) {
    globalOpcodes.lists.ServerZoneIpcType.push({
      name: '_GH_CEDirector',
      opcode: parseInt(ceDirector[1], 16)
    })
  }

  const workerData = await request('https://raw.githubusercontent.com/zhyupe/ffxiv-opcode-worker/master/cn-opcodes.csv')
  const cnOpcodes = workerData.toString().split('\n').map(line => {
    const [key, ...args] = line.trim().split(',')
    const valueColumn = args.reduce((val, content, index) => {
      return content ? index : val
    }, 0)

    return [key, parseInt(args[valueColumn], 16)]
  })

  fs.writeFileSync(join(__dirname, '../Cafe.Matcha/Constant/MatchaOpcode.cs'), `// Copyright (c) FFCafe. All rights reserved.
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
`)
})()

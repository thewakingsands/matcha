# Updating game data

Requires Node.js 22 or later. No npm dependencies are needed for the updater.

Run these commands from the Cafe.Matcha repository root:

```sh
node utils/update-data.mjs
node --test utils/update-data.test.mjs
```

The script generates nine JSON files in `Cafe.Matcha/data`: `instance`, `type`,
`roulette`, `territory`, `patch`, `template`, `fate`, `dynamic-event`, and `world`.
Input and output paths are resolved relative to the script, independently of the
current working directory.

## Data sources

- Game sheets come from XIVAPI v2 (`https://xivapi-v2.xivcdn.com/`). Simplified
  Chinese, Traditional Chinese, Korean, English, Japanese, German, and French
  rows are fetched separately and merged. Missing translations remain empty strings.
- FATE locations are read from the `locations` array in
  `utils/cache/fate-locations.json`. Each record supplies a `territoryId` and an
  `lgbPath` used to determine its expansion. When a FATE has multiple records, the
  territory and expansion from the first record are used.
- Each update also writes `utils/data/fates.json`, using the format
  `{ "120": { "territoryId": 148, "patch": 2 } }`. This simplified file retains
  all source mappings, including FATEs excluded from the game data by the blacklist.
- When the raw file is absent, such as in CI, the updater reads the committed
  `utils/data/fates.json`. Invalid or corrupt raw data causes an error rather than
  falling back. Teamcraft downloads and `--refresh-fates` are no longer used.
- Expansion numbers come from the `lgbPath` prefix: `bg/ffxiv/` means 2, and
  `bg/exN/` means N + 2 (for example, `bg/ex3/` means 5). Unrecognized paths cause
  an error. CI uses the saved expansion instead of inferring it from
  `TerritoryType.ExVersion`. FATEs without a location use `location: 0, patch: 0`.
  The `tools/fate-blacklist.data.json` and `※` test-entry filters remain in effect.
- Dynamic events retain the original group mapping: 1 → 920, 2 → 975, and
  3 → 1252. Other groups are skipped. Event keys are territory ID × 1000 + subrow ID.
- Chinese worlds and data centers come from
  [server.json](https://zhyupe.github.io/ffxiv-datamining-worker/server.json),
  overriding the corresponding XIVAPI world entries.

## Generating raw FATE locations from LGB data

Use [Ixion's FATE location exporter](https://github.com/thewakingsands/ixion/blob/main/docs/commands/fate.md)
to extract locations from a local Windows game installation. From the **Ixion
repository root**, run:

```sh
pnpm x fate export-locations "C:/Games/FINAL FANTASY XIV/game" outputs/fate-locations.json
```

Replace the game path with your installation's `game` directory, which must
contain `sqpack`. The exporter reads local game resources, including LGB files;
it does not require a server or download game data. Bundled SaintCoinach
definitions are used by default. Use definitions matching your installed game
version, optionally with `--saintcoinach <definitions-directory>`.

To use EXDSchema instead:

```sh
pnpm x fate export-locations "C:/Games/FINAL FANTASY XIV/game" outputs/fate-locations.json --exd-schema lib/EXDSchema
```

`--exd-schema` and `--saintcoinach` are mutually exclusive.

Copy the exported `outputs/fate-locations.json` into this repository at
`utils/cache/fate-locations.json`, creating the cache directory if necessary.
Then run `node utils/update-data.mjs` from the Cafe.Matcha repository root to
regenerate the game data and `utils/data/fates.json`.

## GitHub Actions

Manually run **Update data** in GitHub Actions and select the target branch.
CI uses the simplified location file. Generated data changes are committed and
pushed to the selected branch; no commit is created when nothing changes.

The raw cache is ignored by Git. Keep `utils/data/fates.json` under version
control so CI can run without the game installation or raw export. All data is
fetched and converted successfully before output files are written.

# 鱼漂统计与上报

在 ACT 的 Matcha 设置中主动开启“启用鱼漂咬钩时间查询及钓鱼数据上报”后生效；默认关闭，与 Universalis 授权独立。服务为 https://api.ffxiv.cyou/fisher/ 。关闭设置会取消请求、清空待上传队列并撤销在线统计。不会补传开启前的记录。

C# 负责请求，悬浮窗只接收 `FishingStatistics` v1 日志事件。查询只发送已确认的钓场 PlaceName ID；上传使用独立 CBOR 对象，仅含钓场、鱼饵、竿时、钓鱼属性、技能状态和鱼获结果，不含角色标识、名称或原始包。`FishingDTO` 不再输出角色 ID；内部角色标识仅用于过滤和会话切换。

## 协议与边界

协议参考固定提交 [ffxiv-fishing-float 13baacb3](https://github.com/ffxiv-cyou/ffxiv-fishing-float/tree/13baacb3e14c2db848855f43d204decdcddc92dd)，独立实现查询校验及 CBOR 编码。`GET /fisher/fish-duration?spot_id=…&min_count=10` 使用 distributions，按鱼饵及撒饵状态筛选，不采用 merged。`POST /fisher/upload` 携带 `X-Client-Timestamp`，数组字段顺序参见 `FishingUpload.Create`。

已确认的钓场目录由 `utils/update-data.mjs` 从 FishingSpot 的 PlaceName 生成。0、950、缺失及目录外 ID 都不查询或上传。每次普通抛竿重新确认钓场；以小钓大只继承仍在进行的钓鱼会话。状态基线、属性、提钩方式或鱼获字段不完整时跳过上报，查询不受影响。

查询超时 5 秒、响应上限 2 MiB；同钓场合并请求，最多缓存 32 个钓场，10 分钟刷新、空结果 5 分钟、24 小时失效。失败至少冷却 60 秒并遵守 Retry-After。停止钓鱼后不重试查询。上传每 60 秒一次，最多 120 竿、10 分钟，只存内存；结果不明确的批次不重发，新记录与在途批次分离。登出、进程切换、卸载及撤销授权清空队列。

## 属性读取兼容性

通过只读进程句柄读取 PlayerState，不注入或调用客户端函数。获得力、鉴别力和装备鱼饵来自 PlayerState；当前/最大 GP 来自 ACT 公开角色列表，允许 GP 为 0。内存线程每 500 ms 采样，抛竿时固定最近 2 秒内的不可变快照。已有换饵、以小钓大或任务鱼饵事件优先于内存补全。

布局来源为 FFXIVClientStructs `25762ad04` 的 PlayerState。通过可执行节中的唯一静态地址签名定位，不再计算或验证 exe 文件 SHA-256。定位仅在进程身份变化时执行。

签名匹配不唯一、角色未加载、进程变化或读取失败均返回未知；保留职业、角色一致性和属性范围检查。字段偏移仍来自上述布局，签名匹配成功不代表新客户端的字段布局已经验证。CN/Global 的既有静态分析验证了地址签名及属性索引读取，CN 的 PlayerSetup 处理验证了鱼饵写入。

钓鱼状态基线仅接入普通 `StatusEffectList`，随后结合 `ActorControl` 状态增减事件维护。不接入未经个人钓鱼场景确认的 `StatusEffectList2/3`；未取得完整状态基线时跳过上传。

## 验证

`dotnet run --project tests/FishingService` 使用人工数据、模拟 HTTP 和人工 PE，不连接真实服务。覆盖授权、迟到响应、未知钓场、缓存和冷却、批次隔离、零 GP、属性快照、包事件关联、拟饵、以小钓大、失败鱼获、签名歧义及 CBOR 契约。`node --test utils/update-data.test.mjs` 验证目录生成。

Windows 实机验收仍需执行，不能以静态分析或 Linux 核心测试代替：

1. 编译完整插件，分别在目标 CN/Global 构建核对获得力、鉴别力、GP、鱼饵，覆盖换装、食物、职业切换和进程切换。
2. 验证默认关闭不请求，主动开启后普通抛竿、换饵、撒饵、拟饵、以小钓大及脱钩行为。
3. 模拟断网及撤销授权，确认计时连续、在线结果撤销，角色切换不遗留队列。
4. 真实上传仅在用户主动开启后检查，不向服务发送人工测试样本。

## 内存读取保护与日志

- 只申请 `PROCESS_VM_READ | PROCESS_QUERY_LIMITED_INFORMATION`，没有写内存、修改页权限、注入、挂钩或远程调用游戏函数的代码。
- 使用 `SafeProcessHandle` 自动释放句柄，并核对句柄对应的创建时间与选中进程一致，防止 PID 复用。
- exe 读取限制为 512 MiB；校验 MZ/PE、AMD64、PE32+、节边界。扫描必须唯一，PlayerState 的读取范围必须完整落在模块内可读、不可执行的数据节。
- 每次读取检查返回值及实际字节数；读取前后核对角色及职业。读取失败、不完整或存在角色切换时返回未知。后台采样互斥，卸载后停止计时器并释放资源。
- 移除文件 hash 白名单后，仍使用已知字段偏移；上述检查不能证明所有未来版本的布局正确。Windows 实机属性对照仍然必要。

新增日志分类为 `Fishing`，沿用 Matcha 日志开关和级别筛选（Release 下查看 Info 需要打开现有 Debug/详细日志选项）。内存扫描记录启动、定位成功或失败；属性记录各字段是否可用，不记录数值；查询记录请求、缓存命中、结果条数、失败类别、HTTP 状态和取消；上传记录排队/批次数量、跳过、过期、成功和失败。高频采样仅在状态变化时输出，日志回调异常不影响采样或网络处理。

日志不包含角色名、角色 ID、进程 ID、文件路径、内存地址、原始包、请求或响应正文，也不输出异常消息及堆栈。

人工测试覆盖无效/截断 PE、签名歧义、模块及数据节越界、未加载角色、职业不匹配、读取期间角色变化、短读、异常隔离、日志去重和隐私边界。进程外读取遇到不可访问地址时由 Windows 返回失败，而非在游戏线程中解引用指针；参见 [ReadProcessMemory](https://learn.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory)。本次未执行 Windows 实机稳定性测试。

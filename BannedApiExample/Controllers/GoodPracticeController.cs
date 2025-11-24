using Microsoft.AspNetCore.Mvc;

namespace BannedApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class GoodPracticeController : ControllerBase
{
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<GoodPracticeController> _logger;

    public GoodPracticeController(TimeProvider timeProvider, ILogger<GoodPracticeController> logger)
    {
        _timeProvider = timeProvider;
        _logger = logger;
    }

    [HttpGet("time")]
    public IActionResult GetTime()
    {
        // ✅ [OK] 推奨: TimeProvider を使用することで、テスト時に時間をモック化できます
        var now = _timeProvider.GetLocalNow();
        return Ok(new { CurrentTime = now });
    }

    [HttpPost("log")]
    public IActionResult LogMessage()
    {
        // ✅ [OK] 推奨: ILogger を使用することで、構造化ログとして出力されます
        _logger.LogInformation("Processing log request at {Time}", _timeProvider.GetLocalNow());
        return Ok("Logged to ILogger");
    }

    [HttpGet("file")]
    public async Task<IActionResult> ReadFile()
    {
        // ✅ [OK] 推奨: 非同期I/Oを使用することで、スレッドをブロックしません
        try
        {
            // 注: 実際のアプリでは IFileProvider などの抽象化レイヤーの使用を検討してください
            if (!System.IO.File.Exists("config.json"))
            {
                return NotFound("File not found");
            }

            var content = await System.IO.File.ReadAllTextAsync("config.json");
            return Ok(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading file");
            return BadRequest(ex.Message);
        }
    }
}

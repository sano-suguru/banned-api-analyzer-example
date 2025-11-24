using Microsoft.AspNetCore.Mvc;

namespace BannedApiExample.Controllers;

[ApiController]
[Route("[controller]")]
public class BadPracticeController : ControllerBase
{
    [HttpGet("time")]
    public IActionResult GetTime()
    {
        // ❌ [NG] 禁止: 以下の行でエラー RS0030 が発生します
        // "現在時刻に依存する処理はテストが困難になるため、TimeProvider を使用してください。"
        var now = DateTime.Now;
        return Ok(new { CurrentTime = now });
    }

    [HttpPost("log")]
    public IActionResult LogMessage()
    {
        // ❌ [NG] 禁止: 以下の行でエラー RS0030 が発生します
        // "コンソール出力はログ基盤に収集されない可能性があるため、ILogger を使用してください。"
        Console.WriteLine("Processing log request...");
        return Ok("Logged to console");
    }

    [HttpGet("file")]
    public IActionResult ReadFile()
    {
        // ❌ [NG] 禁止: 以下の行でエラー RS0030 が発生します
        // "スレッドプールの枯渇を防ぐため、I/O処理には非同期メソッドを使用してください。"
        // 注: ファイルが存在しなくても、Analyzerは使用箇所を検出します
        try
        {
            var content = System.IO.File.ReadAllText("config.json");
            return Ok(content);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

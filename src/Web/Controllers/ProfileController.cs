using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApp.Controllers
{
    public class ProfileController : Controller
    {
        // จำลองการบันทึกข้อมูล "Bio" (แนะนำตัว) ของผู้ใช้
        [HttpPost]
        public IActionResult UpdateUserBio(string userId, string rawBioContent)
        {
            // ❌ VULNERABILITY: Stored Cross-Site Scripting (XSS) (CWE-79)
            // ปัญหา: รับข้อมูลจากผู้ใช้ (rawBioContent) แล้วบันทึกลงฐานข้อมูล "ทันที"
            // โดยไม่มีการทำ "Sanitization" หรือ "HTML Encoding" ป้องกันแท็กอันตราย

            // สมมติว่าบรรทัดนี้คือการ Save ลง DB จริงๆ
            // _dbContext.Users.Update(u => u.Id == userId, u => u.Bio = rawBioContent);

            // โค้ดจำลองเพื่อให้ Build ผ่าน (AI จะจับที่ตัวแปร rawBioContent ที่ถูกนำไปใช้ตรงๆ)
            string dataToSave = rawBioContent;
            Console.WriteLine($"Saving bio for user {userId}: {dataToSave}");

            return Ok("Bio updated successfully! (But insecurely...)");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pisang.Models;
using pisang.Data;

namespace pisang.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TblUserController : ControllerBase
{
    private readonly UserContext DbContext;

    public TblUserController(UserContext DbContext)
    {
        this.DbContext = DbContext;
    }

    [HttpGet]
    public async Task<ActionResult> GetTblUsers()
    {
        return Ok(await DbContext.TblUsersBase.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult> UserCred(UserCred userCred)
    {
        var UserCred = new TblUsers()
        {
            Nama = userCred.Nama,
            Password = userCred.Password
        };
        
        await DbContext.TblUsersBase.AddAsync(UserCred);
        await DbContext.SaveChangesAsync();

        return Ok(UserCred);
    }

    private bool UserCredExists(string username)
    {
        return DbContext.TblUsersBase.Any(e => e.Nama == username);
    }

    private static TblUsers ItemToDTO(TblUsers tblUsers) =>
       new TblUsers
       {
           Id = tblUsers.Id,
           Nama = tblUsers.Nama,
           Email = tblUsers.Email,
           Password = tblUsers.Password,
       };
}
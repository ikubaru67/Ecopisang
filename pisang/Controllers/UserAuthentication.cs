using Microsoft.AspNetCore.Mvc;
using pisang.Models;
using pisang.Data;
using pisang.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace pisang.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserContext DbContext;
    private readonly JwtSettings jwtSettings;
    public UserController(UserContext userContext,IOptions<JwtSettings> options)
    {
        this.DbContext = userContext;
        this.jwtSettings = options.Value;
    }

    [HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromBody]Register register)
    {
        var user = await this.DbContext.TblUsersBase.FirstOrDefaultAsync(item=> item.Email==register.Email && item.Password==register.password);
        if (user == null)
            return Unauthorized();
        //Generate Token
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(this.jwtSettings.securitykey);
        var tokendesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[] { new Claim(ClaimTypes.Name, user.Nama) }
            ),
            Expires = DateTime.Now.AddSeconds(20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokendesc);
        string finaltoken = tokenhandler.WriteToken(token);

        return Ok(finaltoken);
    }

    [HttpPost]
    public async Task<ActionResult> UserCred(UserCred userCred)
    {
        var UserCred = new TblUsers()
        {
            Nama = userCred.Nama,
            Email = userCred.Email,
            Password = userCred.Password,
            Telepon = userCred.Telepon,
            Alamat = userCred.Alamat
        };
        
        await DbContext.TblUsersBase.AddAsync(UserCred);
        await DbContext.SaveChangesAsync();

        return Ok(UserCred);
    }

    [HttpGet]
    public async Task<ActionResult> GetTblUsers()
    {
        return Ok(await DbContext.TblUsersBase.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TblUsers>> GetTblUsers(int id)
    {
        var TblUser = await DbContext.TblUsersBase.FindAsync(id);

        if (TblUser == null)
        {
            return NotFound();
        }

        return Ok(TblUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTblUsers(int id, TblUsers tblUsers)
    {
        if (id != tblUsers.Id)
        {
            return BadRequest();
        }

        var TblUsers = await DbContext.TblUsersBase.FindAsync(id);
        if (TblUsers == null)
        {
            return NotFound();
        }

        TblUsers.Nama = tblUsers.Nama;
        TblUsers.Email = tblUsers.Email;
        TblUsers.Password = tblUsers.Password;
        TblUsers.Telepon = tblUsers.Telepon;
        TblUsers.Alamat = tblUsers.Alamat;

        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TblUsersExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    private bool TblUsersExists(int id)
    {
        return DbContext.TblUsersBase.Any(e => e.Id == id);
    }

    private static TblUsers ItemToDTO(TblUsers tblUser) =>
       new TblUsers
       {
        Id = tblUser.Id,
        Nama = tblUser.Nama,
        Password = tblUser.Password,
        Email = tblUser.Email,
        Telepon = tblUser.Telepon,
        Alamat = tblUser.Alamat
       };
}
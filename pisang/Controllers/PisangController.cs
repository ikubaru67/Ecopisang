using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pisang.Models;
using pisang.Data;
using Microsoft.AspNetCore.Authorization;

namespace Durian.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PisangController : ControllerBase
{
    private readonly UserContext DbContext;

    public PisangController(UserContext DbContext)
    {
        this.DbContext = DbContext;
    }


    [HttpGet]
    public async Task<ActionResult> GetPisang()
    {
        return Ok(await DbContext.PisangsBase.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pisangs>> GetPisang(int id)
    {
        var PisangItem = await DbContext.PisangsBase.FindAsync(id);

        if (PisangItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(PisangItem);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPisang(int id, Pisangs pisangs)
    {
        if (id != pisangs.Id)
        {
            return BadRequest();
        }

        var PisangItem = await DbContext.PisangsBase.FindAsync(id);
        if (PisangItem == null)
        {
            return NotFound();
        }

        PisangItem.Nama = pisangs.Nama;
        PisangItem.Harga = pisangs.Harga;
        PisangItem.Stok = pisangs.Stok;

        try
        {
            await DbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!PisangItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpPost]
    public async Task<ActionResult> AddPisang(AddPisang addPisang)
    {
        var PisangItem = new Pisangs()
        {
            Nama = addPisang.Nama,
            Harga = addPisang.Harga,
            Stok = addPisang.Stok
        };
        
        await DbContext.PisangsBase.AddAsync(PisangItem);
        await DbContext.SaveChangesAsync();

        return Ok(PisangItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePisang(int id)
    {
        var PisangItem = await DbContext.PisangsBase.FindAsync(id);
        if (PisangItem == null)
        {
            return NotFound();
        }

        DbContext.PisangsBase.Remove(PisangItem);
        await DbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool PisangItemExists(int id)
    {
        return DbContext.PisangsBase.Any(e => e.Id == id);
    }

    private static Pisangs ItemToDTO(Pisangs pisangItem) =>
       new Pisangs
       {
           Id = pisangItem.Id,
           Nama = pisangItem.Nama,
           Harga = pisangItem.Harga,
           Stok = pisangItem.Stok
       };
}
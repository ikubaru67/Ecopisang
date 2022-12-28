using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pisang.Models;
using pisang.Data;

namespace pisang.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PembeliItemsController : ControllerBase
{
    private readonly UserContext dbContext;

    public PembeliItemsController(UserContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET: api/PisangItems
    [HttpGet]
    public async Task<ActionResult> GetPembeliItems()
    {
        return Ok(await dbContext.pembeliItemDTOs.ToListAsync());
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<PembeliItemDTO>> GetPembeliItem(int id)
    {
        var pembeliItem = await dbContext.pembeliItemDTOs.FindAsync(id);

        if (pembeliItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(pembeliItem);
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPembeliItem(int id, PembeliItemDTO pembeliDTO)
    {
        if (id != pembeliDTO.Id)
        {
            return BadRequest();
        }

        var pembeliItem = await dbContext.pembeliItemDTOs.FindAsync(id);
        if (pembeliItem == null)
        {
            return NotFound();
        }

        pembeliItem.Nama = pembeliDTO.Nama;
        pembeliItem.Alamat = pembeliDTO.Alamat;
        pembeliItem.Email = pembeliDTO.Email;
        pembeliItem.Password = pembeliDTO.Password;
        pembeliItem.Nomor = pembeliDTO.Nomor;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!PembeliItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult> AddPembeli(AddPembeli addPembeli)
    {
        var pembeliItem = new PembeliItemDTO()
        {
            Nama = addPembeli.Nama,
            Alamat = addPembeli.Alamat,
            Email = addPembeli.Email,
            Password = addPembeli.Password,
            Nomor = addPembeli.Nomor
        };
        
        await dbContext.pembeliItemDTOs.AddAsync(pembeliItem);
        await dbContext.SaveChangesAsync();

        return Ok(pembeliItem);
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePembeliItem(int id)
    {
        var pembeliItem = await dbContext.pembeliItemDTOs.FindAsync(id);
        if (pembeliItem == null)
        {
            return NotFound();
        }

        dbContext.pembeliItemDTOs.Remove(pembeliItem);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool PembeliItemExists(int id)
    {
        return dbContext.pembeliItemDTOs.Any(e => e.Id == id);
    }

    private static PembeliItemDTO ItemToDTO(PembeliItemDTO pembeliItem) =>
       new PembeliItemDTO
       {
           Id = pembeliItem.Id,
           Nama = pembeliItem.Nama,
           Alamat = pembeliItem.Alamat,
           Email = pembeliItem.Email,
           Password = pembeliItem.Password,
           Nomor = pembeliItem.Nomor
       };
}
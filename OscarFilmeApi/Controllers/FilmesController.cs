using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OscarFilmeApi.Data;
using OscarFilmeApi.Models;

namespace OscarFilmeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmesController(AppDbContext context) : ControllerBase
{
    private const int PrimeiroAnoOscar = 1929;

    [HttpPost]
    public async Task<ActionResult<Filme>> Post(Filme filme)
    {
        if (filme.AnoLancamento < PrimeiroAnoOscar)
        {
            return BadRequest("O ano de lancamento nao pode ser menor que 1929, ano do primeiro Oscar.");
        }

        context.Filmes.Add(filme);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = filme.Id }, filme);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Filme>>> Get()
    {
        var filmes = await context.Filmes.ToListAsync();
        return Ok(filmes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Filme>> GetById(int id)
    {
        var filme = await context.Filmes.FindAsync(id);

        if (filme is null)
        {
            return NotFound();
        }

        return Ok(filme);
    }

    [HttpGet("vencedores")]
    public async Task<ActionResult<IEnumerable<Filme>>> GetVencedores()
    {
        var vencedores = await context.Filmes
            .Where(f => f.Venceu)
            .ToListAsync();

        return Ok(vencedores);
    }

    [HttpGet("estatisticas")]
    public async Task<ActionResult<object>> GetEstatisticas()
    {
        var totalFilmes = await context.Filmes.CountAsync();
        var totalVencedores = await context.Filmes.CountAsync(f => f.Venceu);

        return Ok(new
        {
            TotalFilmes = totalFilmes,
            TotalVencedores = totalVencedores
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Filme filmeAtualizado)
    {
        var filme = await context.Filmes.FindAsync(id);

        if (filme is null)
        {
            return NotFound();
        }

        if (filmeAtualizado.AnoLancamento < PrimeiroAnoOscar)
        {
            return BadRequest("O ano de lancamento nao pode ser menor que 1929, ano do primeiro Oscar.");
        }

        var virouVencedor = !filme.Venceu && filmeAtualizado.Venceu;

        filme.Titulo = filmeAtualizado.Titulo;
        filme.Diretor = filmeAtualizado.Diretor;
        filme.Categoria = filmeAtualizado.Categoria;
        filme.AnoLancamento = filmeAtualizado.AnoLancamento;
        filme.Venceu = filmeAtualizado.Venceu;

        await context.SaveChangesAsync();

        if (virouVencedor)
        {
            Console.WriteLine($"Temos um novo vencedor: {filme.Titulo}!");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var filme = await context.Filmes.FindAsync(id);

        if (filme is null)
        {
            return NotFound();
        }

        context.Filmes.Remove(filme);
        await context.SaveChangesAsync();

        return NoContent();
    }
}

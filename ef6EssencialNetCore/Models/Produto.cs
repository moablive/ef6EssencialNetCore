using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ef6EssencialNetCore.Validations;

namespace ef6EssencialNetCore.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage="o nome é obrigatorio")]
    [StringLength(80, ErrorMessage="o nome deve ter no Maximo {1} caracteres e no minimo {2} caracteres", MinimumLength=5)]
    [PrimeiraLetraMaiuscula] 
    public string? Nome { get; set; }

    [Required]
    [StringLength(300, ErrorMessage="A Descricao deve ter no Maximo {1} caracteres")]
    public string? Descricao { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName="decimal(8,2)")]
    [Range(1,10000, ErrorMessage ="O Preço deve Estar entre {1} e {2}")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300, MinimumLength=10)]
    public string? ImagemUrl { get; set; }

    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    //Relacionamento
    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

}

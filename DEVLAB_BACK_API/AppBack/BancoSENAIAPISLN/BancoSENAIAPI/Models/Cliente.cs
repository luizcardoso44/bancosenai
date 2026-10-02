using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        [Key]
        public int CodigoCliente { get; set; }
        [Required]
        public required string NomeCliente { get; set; }
        [Required]
        public required string Cpf {  get; set; }
        [Required]
        public int NumeroAgencia { get; set; }
        [Required]
        public int SaldoTotal { get; set; }
        [Required]
        public required string Sexo {  get; set; }
        [Required]
        public required string Endereço { get; set; }
        [Required]
        public required string cidade { get; set; }
        [Required]
        public required string estado { get; set;}

    }
}

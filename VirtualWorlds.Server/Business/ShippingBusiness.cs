using VirtualWorlds.Server.Models;

namespace VirtualWorlds.Server.Business
{
    public static partial class ShippingBusiness
    {
        public static bool ValidateBookIds(List<int>? bookIds, out string? error)
        {
            if (bookIds == null || bookIds.Count == 0)
            {
                error = "Informe ao menos um código de livro.";
                return false;
            }

            error = null;
            return true;
        }

        public static bool ValidateBooksFound(List<object> books, out string? error)
        {
            if (books == null || !books.Any())
            {
                error = "Nenhum livro encontrado com os códigos informados.";
                return false;
            }

            error = null;
            return true;
        }
    }
}

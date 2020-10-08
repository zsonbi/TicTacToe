using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IAI
    {
        bool Side { get; }

        Task<byte[]> next();
    }
}
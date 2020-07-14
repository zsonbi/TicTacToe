using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IAI
    {
        bool Side { get; }

        Task<byte[]> next();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    interface IAI
    {
        bool aiside { get; set; } 

        Task<byte[]> next(); 
    }
}

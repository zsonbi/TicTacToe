using System;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class ameba
    {
        //Varriables

        private State gameState; //The gameboard basicly, but it can also check if someone won
        private IAI ai; //ai WOW it must be smart, no it's not
        private byte aiType;
        private IAI otherai; //ai if the user want a 1v1 ai match
        private byte playAgainstItselfSide = 1;
        private bool isThereTwoAI = false;

        //**********************************************************
        //Properties
        //Returns if the game is over
        public bool isOver { get => gameState.isOver; }

        //Get the cells which won the game
        public byte[][] winnerCells { get => gameState.WinnerCells.ToArray(); }

        //Static Properties
        //x size of the board default 3
        public static byte X { get; private set; }

        //y size of the board default 3
        public static byte Y { get; private set; }

        //How many 'X' or 'O' do the players need to win
        public static byte Checksize { get; private set; }

        //the current playing side true 'X' false 'O'
        public bool Side { get; private set; }

        //Constructor
        public ameba(byte x = 3, byte y = 3, byte checksize = 3, bool aiControlled = false, bool aiSide = false, byte aiType = 1)
        {
            X = x;
            Y = y;
            Checksize = checksize;
            Side = true;
            gameState = new State();
            this.aiType = aiType;
            if (aiControlled)
            {
                ai = CreateAi(aiSide);
            }//if
        }

        //Consturctor if we want to pit two ai's against eachother
        public ameba(byte aiType, byte x = 3, byte y = 3, byte checksize = 3)
        {
            X = x;
            Y = y;
            Checksize = checksize;
            Side = true;
            gameState = new State();
            this.aiType = aiType;
            ai = CreateAi(true);
            otherai = CreateAi(false);
            isThereTwoAI = true;
        }

        //**************************************************************************
        //Private method
        //Creates the apropiate ai
        private IAI CreateAi(bool aiSide)
        {
            IAI outputAI;
            switch (aiType)
            {
                case 0:
                    outputAI = new MiniMaxAI((byte)(aiSide ? 1 : 2), gameState);
                    break;

                case 1:
                    outputAI = new MCTSAI((byte)(aiSide ? 1 : 2), gameState);
                    break;

                case 2:
                    outputAI = new PatternAI((byte)(aiSide ? 1 : 2), gameState);
                    break;

                default:
                    outputAI = new MiniMaxAI((byte)(aiSide ? 1 : 2), gameState);
                    break;
            }
            return outputAI;
        }

        //**************************************************************************
        //Public method
        //Changes the indexed element to the side which comes next
        public async Task Change(byte y, byte x)
        {
            gameState.Change(new Action(new byte[] { y, x }, (byte)(Side ? 1 : 2)));
            Side = !Side;
        }

        //------------------------------------------------------------------
        //Gets the next move of the bot
        public async Task<IAction> Next()
        {
            return await Task.Run(() => ai.Next());
        }

        //--------------------------------------------------------------------------------------------
        //Plays a game with two ai against each other
        public async Task<IAction> PlayAgainstIstelfNextMove()
        {
            if (!isThereTwoAI)
                throw new Exception("Used wrong Constructor there is no otherai");
            IAction nextMove = await Task.Run(() => Side ? ai.Next() : otherai.Next());
            await this.Change(nextMove.Move[0], nextMove.Move[1]);
            return nextMove;
        }
    }
}
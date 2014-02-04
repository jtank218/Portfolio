using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Timers;

using Sanford.Multimedia.Midi;

namespace SampleMidiUse
{
    class Program
    {
        public enum TurnState { PlayerTurn, EnemyTurn, GameOver, GameStart };
        public static TurnState TS;

        

        static void Main(string[] args)
        {
            TS = TurnState.GameStart;


            StartGame();
        }

        public static void StartGame()
        {
            Microphone mic= new Microphone();
            Player p1 = new Player(100, 10);
            Character enemy = new Character(100, 5);
            bool defend = false;
            bool playerWin = false;
            while (!playerWin)
            {

                    switch (TS)
                    {
                        case (TurnState.GameStart):

                            mic.Init();
                            mic.FindRange();

                            while (mic.IsOn()) { }

                            TS = TurnState.PlayerTurn;
                            break;

                        case (TurnState.PlayerTurn):

                            defend = false;

                            Console.Write("\nYour Turn!\n1:Defend\n2:Attack\n\nChoice:");

                            string choice = Console.ReadLine();

                            while (choice.Length != 1 && (int.Parse(choice) != 1 || int.Parse(choice) != 2))
                            {
                                Console.WriteLine("Incorrect.  Please try again:");
                                choice = Console.ReadLine();
                            }

                            int action = int.Parse(choice);

                            if (action == 1)
                            {
                                defend = true;
                                TS = TurnState.EnemyTurn;
                            }

                            else if(action == 2)
                            {
                                //compare note sung to note desired
                                int desNote = mic.RandomNote();

                                Console.WriteLine("\n" + "Please sing: " + mic.getNoteLetter(desNote));

                                mic.setCurrentNote(0);

                                mic.StartRecording();

                                while (mic.getCurrentNote() != desNote + 1 && mic.getCurrentNote() != desNote - 1 && mic.getCurrentNote() != desNote) {}

                                mic.StopRecording();

                                enemy.takeDamage(p1.dealDamage(Math.Abs(desNote - mic.getCurrentNote())));

                                Console.WriteLine("Enemy Health: " + enemy.getHealth());

                                if (enemy.getHealth() <= 0)
                                {
                                    playerWin = true;
                                    TS = TurnState.GameOver;
                                }
                                else
                                {
                                    TS = TurnState.EnemyTurn;
                                }
                            }

                            

                            break;
                        case (TurnState.EnemyTurn):

                            p1.takeDamage(enemy.dealDamage(defend));

                            Console.WriteLine("Player Health: " + p1.getHealth() + "\n");

                            if (p1.getHealth() > 0)
                            {
                                TS = TurnState.PlayerTurn;
                            }
                            else
                            {
                                playerWin = false;
                                TS = TurnState.GameOver;
                            }
                            break;
                        case (TurnState.GameOver):
                            if (playerWin)
                            {
                                Console.WriteLine("CONGRATS!!!!");
                            }
                            else
                            {
                                Console.WriteLine("Oh too bad.  So sad.");
                            }
                            break;
                    }
            }
        }
    }
}

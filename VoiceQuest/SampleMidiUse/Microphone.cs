using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Timers;

using Sanford.Multimedia.Midi;

namespace SampleMidiUse
{
    class Microphone
    {
        public enum MicState { TopNote, BottomNote, Silence, Listening };

        public static MicState MC = MicState.Silence;

        private int topNote = 21;
        private int bottomNote = 94;
        private int currentNote = 0;

        private bool listening = false;

        InputDevice inpt = new InputDevice(0);
        ChannelStopper stopper = new ChannelStopper();
        private static Timer longTimer = new Timer(5000);

        public int getTopNote() { return topNote; }
        public int getBottomNote() { return bottomNote; }

        public int getCurrentNote() { return currentNote; }
        public void setCurrentNote(int n) { currentNote = n; }

        public bool IsOn() { return listening; }

        public Microphone()
        {
        }

        public void Init()
        {
            longTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            inpt.ChannelMessageReceived += delegate(object sender, ChannelMessageEventArgs e)
            {
                stopper.Process(e.Message);
                //Console.Out.WriteLine(e.Message.Data1);
                //Console.Out.WriteLine(e.Message.Data2);

                switch (MC)
                {
                    case MicState.Listening:
                        currentNote = e.Message.Data1;
                        Console.WriteLine(getNoteLetter(currentNote));
                        break;
                    case MicState.Silence:
                        break;
                    case MicState.TopNote:
                        if (e.Message.Data1 > topNote) { topNote = e.Message.Data1; }
                        break;
                    case MicState.BottomNote:
                        if (e.Message.Data1 < bottomNote) { bottomNote = e.Message.Data1; }
                        break;
                    default:
                        Console.WriteLine("OH GOD WHAT HAPPENED!?!?!?!?!");
                        break;

                }
            };

        }

#region getNoteLetter(int note)
        public string getNoteLetter(int note)
        {
            switch(note)
            {
                case 0:
                case 12:
                case 24:
                case 36:
                case 48:
                case 60:
                case 72:
                case 84:
                case 96:
                case 108:
                case 120:
                    return "C";
                case 1:
                case 13:
                case 25:
                case 37:
                case 49:
                case 61:
                case 73:
                case 85:
                case 97:
                case 109:
                case 121:
                    return "C#";
                case 2:
                case 14:
                case 26:
                case 38:
                case 50:
                case 62:
                case 74:
                case 86:
                case 98:
                case 110:
                case 122:
                    return "D";
                case 3:
                case 15:
                case 27:
                case 39:
                case 51:
                case 63:
                case 75:
                case 87:
                case 99:
                case 111:
                case 123:
                    return "D#";
                case 4:
                case 16:
                case 28:
                case 40:
                case 52:
                case 64:
                case 76:
                case 88:
                case 100:
                case 112:
                case 124:
                    return "E";
                case 5:
                case 17:
                case 29:
                case 41:
                case 53:
                case 65:
                case 77:
                case 89:
                case 101:
                case 113:
                case 125:
                    return "F";
                case 6:
                case 18:
                case 30:
                case 42:
                case 54:
                case 66:
                case 78:
                case 90:
                case 102:
                case 114:
                case 126:
                    return "F#";
                case 7:
                case 19:
                case 31:
                case 43:
                case 55:
                case 67:
                case 79:
                case 91:
                case 103:
                case 115:
                case 127:
                    return "G";
                case 8:
                case 20:
                case 32:
                case 44:
                case 56:
                case 68:
                case 80:
                case 92:
                case 104:
                case 116:
                    return "G#";
                case 9:
                case 21:
                case 33:
                case 45:
                case 57:
                case 69:
                case 81:
                case 93:
                case 105:
                case 117:
                    return "A";
                case 10:
                case 22:
                case 34:
                case 46:
                case 58:
                case 70:
                case 82:
                case 94:
                case 106:
                case 118:
                    return "A#";
                case 11:
                case 23:
                case 35:
                case 47:
                case 59:
                case 71:
                case 83:
                case 95:
                case 107:
                case 119:
                    return "B";
                default:
                    return ""+note+"";
            }

            //return new char();
        }
#endregion 
       
        public void FindRange()
        {
            inpt.StartRecording();
            listening = true;
            longTimer.Start();
            MC = MicState.BottomNote;
            Console.WriteLine("Please sing your lowest note\n");
        }

        public int RandomNote()
        {
            int note = 0;

            note = (int)Math.Round((double)(bottomNote + ((topNote - bottomNote) / 2)));

            return note;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            switch (MC)
            {
                case MicState.TopNote:
                    MC = MicState.Silence;
                    longTimer.Stop();
                    Console.WriteLine( "High Note: " + getTopNote() );
                    Console.WriteLine( "Low Note: " + getBottomNote() );
                    listening = false;
                    break;
                case MicState.BottomNote:
                    Console.Out.WriteLine("Please sing your highest note\n");
                    MC = MicState.TopNote;
                    break;
                default:
                    Console.Out.WriteLine("Something Broke");
                    break;
            }
        }

        public void StartRecording()
        {
            listening = true;
            MC = MicState.Listening;
            inpt.StartRecording();
        }

        public void StopRecording()
        {
            listening = false;
            MC = MicState.Silence;
            inpt.StopRecording();
        }

    }
}

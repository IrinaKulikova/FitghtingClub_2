﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitghtingClub_WPF
{
    class ViewModelGame : INotifyPropertyChanged
    {
        private ILogger _logger;
        private IGame _game;

        //поле хранит данные для кого отображается игра, для Player или AIPlayer
        public bool IsAIPlayer { get; set; }

        public ObservableCollection<BasePlayer> Players { get; set; }

        public int CurrentPlayer
        {
            get => _game.CurrentPlayer;
            set
            {
                _game.CurrentPlayer = value;
                OnPropertyChanged("CurrentPlayer");
            }
        }

        public String Log
        {
            get => _logger.Log;
            set
            {
                _logger.Log = value;
                OnPropertyChanged("Log");
            }
        }

        public String CurrentPlayerName
        {
            get => _game.Players[CurrentPlayer].Name;
            set
            {
                _game.Players[CurrentPlayer].Name = value;
                OnPropertyChanged("CurrentPlayerName");
            }
        }

        public int Round
        {
            get => _game.Round;
            set
            {
                _game.Round = value;
                OnPropertyChanged("Round");
            }
        }

        private ICommand _commandBlockHead;
        public ICommand CommandBlockHead
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandBlockHead = _commandBlockHead ?? new CommandBlock(BodyPart.Head);
                return _commandBlockHead;
            }
        }

        private ICommand _commandBlockTrunk;
        public ICommand CommandBlockTrunk
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandBlockTrunk = _commandBlockTrunk ?? new CommandBlock(BodyPart.Trunk);
                return _commandBlockTrunk;
            }
        }

        private ICommand _commandBlockLegs;
        public ICommand CommandBlockLegs
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandBlockLegs = _commandBlockLegs ?? new CommandBlock(BodyPart.Legs);
                return _commandBlockLegs;
            }
        }

        private ICommand _commandNewGame;
        public ICommand CommandNewGame
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandNewGame = _commandNewGame ?? new CommandNewGame();
                return _commandNewGame;
            }
        }

        private ICommand _commandHitHead;
        public ICommand CommandHitHead
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandHitHead = _commandHitHead ?? new CommandHit(BodyPart.Head);
                return _commandHitHead;
            }
        }

        private ICommand _commandHitTrunk;
        public ICommand CommandHitTrunk
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandHitTrunk = _commandHitTrunk ?? new CommandHit(BodyPart.Trunk);
                return _commandHitTrunk;
            }
        }

        private ICommand _commandHitLegs;
        public ICommand CommandHitLegs
        {
            get
            {
                if (IsAIPlayer)
                {
                    return new CommandEmpty();
                }
                _commandHitLegs = _commandHitLegs ?? new CommandHit(BodyPart.Legs);
                return _commandHitLegs;
            }
        }

        public ViewModelGame()
        {
            _game = Game.GetInstance();
            _logger = HistoryLogger.GetInstance();
            _logger.PropertyChanged += Model_PropertyChanged;
            _game.PropertyChanged += Model_PropertyChanged;
            _game.NewGameEvent += _game_NewGameEvent;
            _game.BlockEvent += _game_BlockEvent;
            _game.DeathEvent += _game_DeathEvent;
            _game.WoundEvent += _game_WoundEvent;
            _game.ProtectedEvent += _game_ProtectedEvent;
            Players = new ObservableCollection<BasePlayer>(_game.Players);
        }

        private void _game_WoundEvent(object sender, EventArgsWound e)
        {
            _logger.Log = (sender as BasePlayer).Name + " hit in the " + e.Part;
        }

        private void _game_ProtectedEvent(object sender, EventArgsProtected e)
        {
            _logger.Log = (sender as BasePlayer).Name + " protected " + e.Part;
        }

        private void _game_NewGameEvent(object sender, EventArgs e)
        {
            _logger.Log = "New game!";
        }

        private void _game_DeathEvent(object sender, EventArgsDeath e)
        {
            _logger.Log = (sender as BasePlayer).Name + " died!!!";
        }

        private void _game_BlockEvent(object sender, EventArgsBlock e)
        {
            _logger.Log = (sender as BasePlayer).Name + " set block " + e.Part;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
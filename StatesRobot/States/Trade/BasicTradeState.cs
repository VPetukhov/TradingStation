﻿using System;
using System.Linq;
using StatesRobot.States.End;
using Utils.Events;
using Utils.Types;

namespace StatesRobot.States.Trade
{
	class BasicTradeState : IState
	{
		protected int StartPrice { get; private set; }
		protected bool IsTrendLong { get; private set; }

		public BasicTradeState(int startPrice, bool isTrendLong)
		{
			StartPrice = startPrice;
			IsTrendLong = isTrendLong;
		}

		public virtual ITradeEvent Process(RobotContext context, Candle candle)
		{
			if (candle.Time >= context.EndTime)
			{
				context.CurrentState = new EndState();
				return new DealEvent(!IsTrendLong, candle.Close);
			}

			int endPrice;
			if (IsTrendLong && candle.Low <= (endPrice = StartPrice - context.StopLoss) ||
				!IsTrendLong && candle.High >= (endPrice = StartPrice + context.StopLoss))
			{
				context.CurrentState = new EndState();
				return new StopLossEvent(IsTrendLong, endPrice);
			}
			
			return null;
		}

		public virtual ITradeEvent StopTrading(RobotContext context)
		{
			context.CurrentState = new EndState();
			return new DealEvent(!IsTrendLong, context.Candles.Last().Close);
		}
	}
}

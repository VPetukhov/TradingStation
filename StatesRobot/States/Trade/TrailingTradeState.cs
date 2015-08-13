﻿using TradeTools;
using TradeTools.Events;
using Utils.Events;

namespace StatesRobot.States.Trade
{
	class TrailingTradeState : BreakevenTradeState
	{
		public TrailingTradeState(RobotContext context, Deal deal) : base(context, deal)
		{}

		public override ITradeEvent Process(RobotContext context, Candle candle)
		{
			var result = base.Process(context, candle);
			if (result != null)
				return result;

			if (TrendIsLong)
			{
				if (candle.High - context.StopLossPrice >= context.TrailingStopLoss)
				{
					var newPrice = GetStopPrice(candle.High, context.TrailingStopLoss);
					if (newPrice > context.StopLossPrice)
						return new StopLossMovingEvent(context.StopLossPrice, TrendIsLong);
				}
			}
			else 
			{
				if (context.StopLossPrice - candle.Low >= context.TrailingStopLoss)
				{
					var newPrice = GetStopPrice(candle.Low, context.TrailingStopLoss);
					if (newPrice < context.StopLossPrice)
						return new StopLossMovingEvent(context.StopLossPrice, TrendIsLong);
				}
			}
			
			return null;
		}
	}
}

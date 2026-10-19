namespace clay.PhilipTheMechanic;
public sealed class Priorities {
	public static readonly double REMOVE_ALL_ACTIONS = double.MaxValue;

	public static readonly double MODIFY_DATA_UNFAVORABLE = 51;
	public static readonly double MODIFY_DATA_FAVORABLE = 50;

	public static readonly double ADD_ACTION = 10;
	public static readonly double MANIPULATE_ACTIONS = 7;
	public static readonly double ADD_ACTION_LAST = 5;
	public static readonly double MODIFY_ACTIONS = -1;
}
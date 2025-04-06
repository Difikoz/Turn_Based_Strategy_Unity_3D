using System;
using UnityEngine;

namespace WinterUniverse
{
    public static class EventBus
    {
        public static Action<Vector2> OnCursorMoved;
        public static Action<int, int> OnTimeChanged;
        public static Action<int, int, int> OnDateChanged;

        public static void CursorMoved(Vector2 position)
        {
            OnCursorMoved?.Invoke(position);
        }

        public static void TimeChanged(int hour, int minute)
        {
            OnTimeChanged?.Invoke(hour, minute);
        }

        public static void DateChanged(int day, int month, int year)
        {
            OnDateChanged?.Invoke(day, month, year);
        }
    }
}
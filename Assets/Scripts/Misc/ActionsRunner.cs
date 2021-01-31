using System;
using System.Collections;

public static class ActionsRunner
{
    public static IEnumerator RunFunctionAsEnumerator(Action actionToDo)
    {
        actionToDo();
        yield return null;
    }
}

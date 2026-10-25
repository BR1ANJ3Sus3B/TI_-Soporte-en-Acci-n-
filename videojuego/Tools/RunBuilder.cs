using System;
using System.Reflection;
using UnityEngine;

public static class RunBuilder
{
    public static string Execute()
    {
        try
        {
            Type type = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType("ReferenceMapBuilder", false);
                if (type != null) break;
            }
            if (type == null) return "TYPE_NOT_FOUND";

            var method = type.GetMethod("BuildFromMenu", BindingFlags.Public | BindingFlags.Static);
            if (method == null) return "METHOD_NOT_FOUND";

            var result = method.Invoke(null, null);
            return "BUILD_OK " + (result ?? "(void)");
        }
        catch (Exception exception)
        {
            var inner = exception is TargetInvocationException target && target.InnerException != null
                ? target.InnerException
                : exception;
            Debug.LogException(inner);
            return "BUILD_ERROR " + inner.GetType().Name + ": " + inner.Message;
        }
    }
}

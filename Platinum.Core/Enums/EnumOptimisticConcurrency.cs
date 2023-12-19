// <copyright file="EnumOptimisticConcurrency.cs" company="Anhny010920">
// Copyright (c) Anhny010920. All rights reserved.
// </copyright>

namespace Platinum.Core.Enums
{
    public enum EnumOptimisticConcurrency
    {
        DatabaseWins, // allow the system/application to discard the client/UI changes and override the update operation using the latest data loaded from the database
        ClientWins, // the system/application to override the database changes with the client/UI changes and update the data into the database
    }
}

// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Network.Structures
{
    /// <summary>
    /// An interface that represents market board purchase information. This message is received from the
    /// server when a purchase is made at a market board.
    /// </summary>
    public interface IMarketBoardPurchase
    {
        /// <summary>
        /// Gets the item ID of the item that was purchased.
        /// </summary>
        uint CatalogId { get; }

        /// <summary>
        /// Gets the quantity of the item that was purchased.
        /// </summary>
        uint ItemQuantity { get; }
    }
}

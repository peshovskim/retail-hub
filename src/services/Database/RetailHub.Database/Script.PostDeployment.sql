/*
  Runs after dacpac publish. Per-table seeds live under Tables/{Entity}/Seed/ as {Entity}.Test.Seed.sql.
*/

:r .\Tables\User\Seed\User.Test.Seed.sql
:r .\Tables\AspNetRoles\Seed\AspNetRoles.Test.Seed.sql
:r .\Tables\AspNetUserRoles\Seed\AspNetUserRoles.Test.Seed.sql
:r .\Tables\Category\Seed\Category.Test.Seed.sql
:r .\Tables\Product\Seed\Product.Test.Seed.sql
:r .\Tables\Cart\Seed\Cart.Test.Seed.sql
:r .\Tables\Cart\Seed\CartItem.Test.Seed.sql
:r .\Tables\Order\Seed\Order.Test.Seed.sql

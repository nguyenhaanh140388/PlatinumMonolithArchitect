using Microsoft.EntityFrameworkCore;
using Platinum.Catalog.Core.Entities;

namespace Platinum.Catalog.Infrastructure.Configurations
{
    public class PlatinumCatalogConfig
    {
        public static void ConfigureEf(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBook>(entity =>
            {
                entity.ToTable("AddressBook");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");
                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .HasColumnName("company");
                entity.Property(e => e.Countryid).HasColumnName("countryid");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .HasColumnName("firstname");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .HasColumnName("lastname");
                entity.Property(e => e.Postalcode)
                    .HasMaxLength(10)
                    .HasColumnName("postalcode");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");
                entity.Property(e => e.Streetaddress)
                    .HasMaxLength(65)
                    .HasColumnName("streetaddress");

                entity.HasOne(d => d.Customer).WithMany(p => p.AddressBooks)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK_AddressBook_Customers");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Finalprice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("finalprice");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Qty).HasColumnName("qty");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK_Cart_Customers");

                entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_Cart_Products");
            });

            modelBuilder.Entity<CartAttribute>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Productoptionid).HasColumnName("productoptionid");
                entity.Property(e => e.Productoptionvalueid).HasColumnName("productoptionvalueid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Customer).WithMany(p => p.CartAttributes)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK_CartAttributes_Customers");

                entity.HasOne(d => d.Product).WithMany(p => p.CartAttributes)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_CartAttributes_Products");

                entity.HasOne(d => d.Productoption).WithMany(p => p.CartAttributes)
                    .HasForeignKey(d => d.Productoptionid)
                    .HasConstraintName("FK_CartAttributes_ProductsOptions");

                entity.HasOne(d => d.Productoptionvalue).WithMany(p => p.CartAttributes)
                    .HasForeignKey(d => d.Productoptionvalueid)
                    .HasConstraintName("FK_CartAttributes_ProductsOptionsValues");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .HasColumnName("image");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.Order).HasColumnName("order");
                entity.Property(e => e.Parentcatid).HasColumnName("parentcatid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToTable("Configuration");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Configurationgroupid).HasColumnName("configurationgroupid");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");
                entity.Property(e => e.Key)
                    .HasMaxLength(65)
                    .HasColumnName("key");
                entity.Property(e => e.Order).HasColumnName("order");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Title)
                    .HasMaxLength(65)
                    .HasColumnName("title");
                entity.Property(e => e.Value)
                    .HasMaxLength(255)
                    .HasColumnName("value");

                entity.HasOne(d => d.Configurationgroup).WithMany(p => p.Configurations)
                    .HasForeignKey(d => d.Configurationgroupid)
                    .HasConstraintName("FK_Configuration_ConfigurationGroup");
            });

            modelBuilder.Entity<ConfigurationGroup>(entity =>
            {
                entity.ToTable("ConfigurationGroup");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Active).HasColumnName("active");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");
                entity.Property(e => e.Order).HasColumnName("order");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Title)
                    .HasMaxLength(65)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Isocode)
                    .HasMaxLength(5)
                    .HasColumnName("ISOCode");
                entity.Property(e => e.Name)
                    .HasMaxLength(65)
                    .HasColumnName("name");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Currency).WithMany(p => p.Countries)
                    .HasForeignKey(d => d.Currencyid)
                    .HasConstraintName("FK_Countries_Currencies");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsFixedLength()
                    .HasColumnName("code");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Symboleleft)
                    .HasMaxLength(15)
                    .HasColumnName("symboleleft");
                entity.Property(e => e.Symbolright)
                    .HasMaxLength(15)
                    .HasColumnName("symbolright");
                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");
                entity.Property(e => e.Email)
                    .HasMaxLength(110)
                    .HasColumnName("email");
                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .HasColumnName("fax");
                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .HasColumnName("firstname");
                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("gender");
                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .HasColumnName("lastname");
                entity.Property(e => e.Mainaddressid).HasColumnName("mainaddressid");
                entity.Property(e => e.Newsletteropted).HasColumnName("newsletteropted");
                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .HasColumnName("telephone");
            });

            modelBuilder.Entity<CustomerInfo>(entity =>
            {
                entity.ToTable("CustomerInfo");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Accountcreatedon)
                    .HasColumnType("datetime")
                    .HasColumnName("accountcreatedon");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Lastlogon)
                    .HasColumnType("datetime")
                    .HasColumnName("lastlogon");
                entity.Property(e => e.Lastmodifiedon)
                    .HasColumnType("datetime")
                    .HasColumnName("lastmodifiedon");
                entity.Property(e => e.Logoncount).HasColumnName("logoncount");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Customer).WithMany(p => p.CustomerInfos)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK_CustomerInfo_Customers");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Image)
                    .HasMaxLength(65)
                    .HasColumnName("image");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<ManufacturersInfo>(entity =>
            {
                entity.ToTable("ManufacturersInfo");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Manufacturerid).HasColumnName("manufacturerid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasColumnName("url");

                entity.HasOne(d => d.Manufacturer).WithMany(p => p.ManufacturersInfos)
                    .HasForeignKey(d => d.Manufacturerid)
                    .HasConstraintName("FK_ManufacturersInfo_Manufacturers");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Comments).HasColumnName("comments");
                entity.Property(e => e.Currency)
                    .HasMaxLength(5)
                    .HasColumnName("currency");
                entity.Property(e => e.CurrencyValue)
                    .HasColumnType("decimal(16, 6)")
                    .HasColumnName("currency_value");
                entity.Property(e => e.CustomerStreetaddress)
                    .HasMaxLength(65)
                    .HasColumnName("customerStreetaddress");
                entity.Property(e => e.Customercity)
                    .HasMaxLength(50)
                    .HasColumnName("customercity");
                entity.Property(e => e.Customercountry)
                    .HasMaxLength(50)
                    .HasColumnName("customercountry");
                entity.Property(e => e.Customeremail)
                    .HasMaxLength(110)
                    .HasColumnName("customeremail");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Customername)
                    .HasMaxLength(65)
                    .HasColumnName("customername");
                entity.Property(e => e.Customerpostalcode)
                    .HasMaxLength(50)
                    .HasColumnName("customerpostalcode");
                entity.Property(e => e.Customerstate)
                    .HasMaxLength(50)
                    .HasColumnName("customerstate");
                entity.Property(e => e.Customertelephone)
                    .HasMaxLength(50)
                    .HasColumnName("customertelephone");
                entity.Property(e => e.Datepurcahsed)
                    .HasColumnType("datetime")
                    .HasColumnName("datepurcahsed");
                entity.Property(e => e.Deliverycity)
                    .HasMaxLength(50)
                    .HasColumnName("deliverycity");
                entity.Property(e => e.Deliverycountry)
                    .HasMaxLength(50)
                    .HasColumnName("deliverycountry");
                entity.Property(e => e.Deliveryname)
                    .HasMaxLength(65)
                    .HasColumnName("deliveryname");
                entity.Property(e => e.Deliverypostalcode)
                    .HasMaxLength(50)
                    .HasColumnName("deliverypostalcode");
                entity.Property(e => e.Deliverystate)
                    .HasMaxLength(50)
                    .HasColumnName("deliverystate");
                entity.Property(e => e.Deliverystreetaddress)
                    .HasMaxLength(65)
                    .HasColumnName("deliverystreetaddress");
                entity.Property(e => e.Latsmodified)
                    .HasColumnType("datetime")
                    .HasColumnName("latsmodified");
                entity.Property(e => e.Orderdatefinished)
                    .HasColumnType("datetime")
                    .HasColumnName("orderdatefinished");
                entity.Property(e => e.Orderstatus)
                    .HasMaxLength(10)
                    .HasColumnName("orderstatus");
                entity.Property(e => e.Paymentmethodid).HasColumnName("paymentmethodid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Shipingmethodid).HasColumnName("shipingmethodid");
                entity.Property(e => e.Shippingcost)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("shippingcost");

                entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Customerid)
                    .HasConstraintName("FK_Orders_Customers");
            });

            modelBuilder.Entity<OrderProductAttribute>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Optionvalueprice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("optionvalueprice");
                entity.Property(e => e.Orderid).HasColumnName("orderid");
                entity.Property(e => e.Orderproductid).HasColumnName("orderproductid");
                entity.Property(e => e.PricePrefix)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("price_prefix");
                entity.Property(e => e.Productoptiobvalue)
                    .HasMaxLength(50)
                    .HasColumnName("productoptiobvalue");
                entity.Property(e => e.Productoptions)
                    .HasMaxLength(50)
                    .HasColumnName("productoptions");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Order).WithMany(p => p.OrderProductAttributes)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK_OrderProductAttributes_Orders");

                entity.HasOne(d => d.Orderproduct).WithMany(p => p.OrderProductAttributes)
                    .HasForeignKey(d => d.Orderproductid)
                    .HasConstraintName("FK_OrderProductAttributes_OrderProductAttributes");
            });

            modelBuilder.Entity<OrdersProduct>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Finalprice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("finalprice");
                entity.Property(e => e.Orderid).HasColumnName("orderid");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Productname)
                    .HasMaxLength(65)
                    .HasColumnName("productname");
                entity.Property(e => e.Productprice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("productprice");
                entity.Property(e => e.Productqty).HasColumnName("productqty");
                entity.Property(e => e.Productstax)
                    .HasColumnType("decimal(9, 4)")
                    .HasColumnName("productstax");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Order).WithMany(p => p.OrdersProducts)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK_OrdersProducts_Orders");

                entity.HasOne(d => d.Product).WithMany(p => p.OrdersProducts)
                    .HasForeignKey(d => d.Productid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrdersProducts_Products");
            });

            modelBuilder.Entity<OrdersStatus>(entity =>
            {
                entity.ToTable("OrdersStatus");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Image)
                    .HasMaxLength(65)
                    .HasColumnName("image");
                entity.Property(e => e.ManufactureId).HasColumnName("manufactureId");
                entity.Property(e => e.Model)
                    .HasMaxLength(50)
                    .HasColumnName("model");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");
                entity.Property(e => e.Qty).HasColumnName("qty");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.Taxclassid).HasColumnName("taxclassid");
                entity.Property(e => e.Weight)
                    .HasColumnType("decimal(7, 2)")
                    .HasColumnName("weight");
            });

            modelBuilder.Entity<ProductsAttribute>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Optionsid).HasColumnName("optionsid");
                entity.Property(e => e.Optionvaluesid).HasColumnName("optionvaluesid");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");
                entity.Property(e => e.PricePrefix)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasColumnName("price_prefix");
                entity.Property(e => e.Productsid).HasColumnName("productsid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Options).WithMany(p => p.ProductsAttributes)
                    .HasForeignKey(d => d.Optionsid)
                    .HasConstraintName("FK_ProductsAttributes_ProductsOptions");

                entity.HasOne(d => d.Optionvalues).WithMany(p => p.ProductsAttributes)
                    .HasForeignKey(d => d.Optionvaluesid)
                    .HasConstraintName("FK_ProductsAttributes_ProductsOptionsValues");

                entity.HasOne(d => d.Products).WithMany(p => p.ProductsAttributes)
                    .HasForeignKey(d => d.Productsid)
                    .HasConstraintName("FK_ProductsAttributes_Products");
            });

            modelBuilder.Entity<ProductsOption>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<ProductsOptionsValue>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasMaxLength(65)
                    .HasColumnName("name");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<ProductsOptionsValuesMapping>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Optionsid).HasColumnName("optionsid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Valuesid).HasColumnName("valuesid");

                entity.HasOne(d => d.Options).WithMany(p => p.ProductsOptionsValuesMappings)
                    .HasForeignKey(d => d.Optionsid)
                    .HasConstraintName("FK_ProductsOptionsValuesMappings_productsdetail");

                entity.HasOne(d => d.Values).WithMany(p => p.ProductsOptionsValuesMappings)
                    .HasForeignKey(d => d.Valuesid)
                    .HasConstraintName("FK_ProductsOptionsValuesMappings_ProductsOptionsValues");
            });

            modelBuilder.Entity<Productsdetail>(entity =>
            {
                entity.ToTable("productsdetail");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Name)
                    .HasMaxLength(65)
                    .HasColumnName("name");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasColumnName("url");
                entity.Property(e => e.Views).HasColumnName("views");

                entity.HasOne(d => d.Product).WithMany(p => p.Productsdetails)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_productsdetail_Products");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Customerid).HasColumnName("customerid");
                entity.Property(e => e.Customername)
                    .HasMaxLength(65)
                    .HasColumnName("customername");
                entity.Property(e => e.Productid).HasColumnName("productid");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_Reviews_Products");
            });

            modelBuilder.Entity<ReviewsDetail>(entity =>
            {
                entity.ToTable("ReviewsDetail");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Reviewid).HasColumnName("reviewid");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Text).HasColumnName("text");

                entity.HasOne(d => d.Review).WithMany(p => p.ReviewsDetails)
                    .HasForeignKey(d => d.Reviewid)
                    .HasConstraintName("FK_ReviewsDetail_Reviews");
            });

            modelBuilder.Entity<WhoIsOnline>(entity =>
            {
                entity.ToTable("WhoIsOnline");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Fullname)
                    .HasMaxLength(65)
                    .HasColumnName("fullname");
                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(18)
                    .HasColumnName("ipaddress");
                entity.Property(e => e.Lastpageurl)
                    .HasMaxLength(65)
                    .HasColumnName("lastpageurl");
                entity.Property(e => e.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Sessionid)
                    .HasMaxLength(255)
                    .HasColumnName("sessionid");
                entity.Property(e => e.Timeentry)
                    .HasColumnType("datetime")
                    .HasColumnName("timeentry");
            });
        }
    }
}

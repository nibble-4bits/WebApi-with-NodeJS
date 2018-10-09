using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi_with_NodeJS.Models;

namespace WebApi_with_NodeJS.Controllers
{
    public class SaleController : ApiController
    {
        // GET
        [HttpGet]
        public IEnumerable<Sale> GetSalesByProductId(int productId)
        {
            SqlConexion conn = new SqlConexion();
            List<Sale> sales = new List<Sale>();
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", productId)
                };

                conn.PrepararProcedimiento("dbo.sp_GetSaleByProductId", parameters);
                dtr = conn.EjecutarTableReader();

                while (dtr.Read())
                {
                    List<SaleDetail> saleDetail = new List<SaleDetail>();
                    saleDetail.Add(new SaleDetail {
                        Id = int.Parse(dtr["SDId"].ToString()),
                        Product = new Product
                        {
                            Id = int.Parse(dtr["ProductId"].ToString()),
                            Name = dtr["ProductName"].ToString(),
                            UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                        },
                        Quantity = int.Parse(dtr["Qty"].ToString())
                    });

                    sales.Add(new Sale
                    {
                        Id = int.Parse(dtr["SaleId"].ToString()),
                        DateTime = DateTime.Parse(dtr["SaleDate"].ToString()),
                        TotalPrice = decimal.Parse(dtr["SaleTotalPrice"].ToString()),
                        SaleDetail = saleDetail
                    });
                }

                return sales;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // GET
        [HttpGet]
        public IEnumerable<Sale> GetSalesByDate(string date)
        {
            SqlConexion conn = new SqlConexion();
            List<Sale> sales = new List<Sale>();
            int[] splitDate = Array.ConvertAll(date.Split('-'), int.Parse);
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>
                {                                           // year         month          day
                    new SqlParameter("@Date", new DateTime(splitDate[0], splitDate[1], splitDate[2]))
                };

                conn.PrepararProcedimiento("dbo.sp_GetSaleByDate", parameters);
                dtr = conn.EjecutarTableReader();

                bool breakFromOuterLoop = false;
                if (dtr.Read())
                {
                    while (!breakFromOuterLoop)
                    {
                        int currentSaleId = int.Parse(dtr["SaleId"].ToString());
                        DateTime currentDateTime = DateTime.Parse(dtr["SaleDate"].ToString());
                        decimal currentTotalPrice = decimal.Parse(dtr["SaleTotalPrice"].ToString());

                        List<SaleDetail> saleDetail = new List<SaleDetail>();
                        do
                        {
                            saleDetail.Add(new SaleDetail
                            {
                                Id = int.Parse(dtr["SDId"].ToString()),
                                Product = new Product
                                {
                                    Id = int.Parse(dtr["ProductId"].ToString()),
                                    Name = dtr["ProductName"].ToString(),
                                    UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                                },
                                Quantity = int.Parse(dtr["Qty"].ToString())
                            });

                            if (!dtr.Read())
                            {
                                breakFromOuterLoop = true;
                                break;
                            }
                        } while (int.Parse(dtr["SaleId"].ToString()) == currentSaleId);

                        sales.Add(new Sale
                        {
                            Id = currentSaleId,
                            DateTime = currentDateTime,
                            TotalPrice = currentTotalPrice,
                            SaleDetail = saleDetail
                        });
                    }
                }

                return sales;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // POST: api/Sale
        [HttpPost]
        public IHttpActionResult Post([FromBody]Sale sale)
        {
            SqlConexion conn = new SqlConexion();
            List<Sale> sales = new List<Sale>();
            DataTable dataTable = new DataTable();
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                // Se crea la tabla que se pasa por parámetro al stored procedure
                dataTable.Columns.Add("ProductId", typeof(int));
                dataTable.Columns.Add("Quantity", typeof(int));
                foreach (var item in sale.SaleDetail)
                {
                    dataTable.Rows.Add(item.Product.Id, item.Quantity);
                }

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@SaleDetail", dataTable)
                };

                conn.PrepararProcedimiento("dbo.sp_AddSale", parameters);
                dtr = conn.EjecutarTableReader();

                List<SaleDetail> saleDetails = new List<SaleDetail>();
                while (dtr.Read())
                {
                    saleDetails.Add(new SaleDetail
                    {
                        Id = int.Parse(dtr["SDId"].ToString()),
                        Product = new Product
                        {
                            Id = int.Parse(dtr["ProductId"].ToString()),
                            Name = dtr["ProductName"].ToString(),
                            UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                        },
                        Quantity = int.Parse(dtr["Qty"].ToString())
                    });
                }

                Sale insertedSale = new Sale
                {
                    Id = int.Parse(dtr["SaleId"].ToString()),
                    DateTime = DateTime.Parse(dtr["SaleDate"].ToString()),
                    TotalPrice = decimal.Parse(dtr["SaleTotalPrice"].ToString()),
                    SaleDetail = saleDetails
                };

                return Ok(insertedSale); // devuelve la venta insertada
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // PUT: api/Sale/5
        // Los parámetros se envían en la URL
        [HttpPut]
        public IHttpActionResult Put(int id, int quantity)
        {
            SqlConexion conn = new SqlConexion();
            SaleDetail saleDetail = null;
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Quantity", quantity)
                };

                conn.PrepararProcedimiento("dbo.sp_UpdateSaleDetailQuantity", parameters);
                dtr = conn.EjecutarTableReader();

                while (dtr.Read())
                {
                    saleDetail = new SaleDetail
                    {
                        Id = int.Parse(dtr["SDId"].ToString()),
                        Product = new Product
                        {
                            Id = int.Parse(dtr["ProductId"].ToString()),
                            Name = dtr["ProductName"].ToString(),
                            UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                        },
                        Quantity = int.Parse(dtr["Qty"].ToString())
                    };
                }

                return Ok(saleDetail); // devuelve el detalle de venta actualizado
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // DELETE: api/Sale/5
        [HttpDelete]
        [Route("api/sale/delSale")]
        public IHttpActionResult DeleteSale(int id)
        {
            SqlConexion conn = new SqlConexion();
            Sale sale = null;
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id),
                };

                conn.PrepararProcedimiento("dbo.sp_DeleteSale", parameters);
                dtr = conn.EjecutarTableReader();

                List<SaleDetail> saleDetails = new List<SaleDetail>();
                while (dtr.Read())
                {
                    saleDetails.Add(new SaleDetail
                    {
                        Id = int.Parse(dtr["SDId"].ToString()),
                        Product = new Product
                        {
                            Id = int.Parse(dtr["ProductId"].ToString()),
                            Name = dtr["ProductName"].ToString(),
                            UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                        },
                        Quantity = int.Parse(dtr["Qty"].ToString())
                    });
                }

                sale = new Sale
                {
                    Id = int.Parse(dtr["SaleId"].ToString()),
                    DateTime = DateTime.Parse(dtr["SaleDate"].ToString()),
                    TotalPrice = decimal.Parse(dtr["SaleTotalPrice"].ToString()),
                    SaleDetail = saleDetails
                };

                return Ok(sale); // devuelve la venta eliminada
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }

        // DELETE: api/Sale/5
        [HttpDelete]
        [Route("api/sale/delSaleDetail")]
        public IHttpActionResult DeleteSaleDetail(int id)
        {
            SqlConexion conn = new SqlConexion();
            SaleDetail saleDetail = null;
            DataTableReader dtr;

            try
            {
                conn.Conectar(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id),
                };

                conn.PrepararProcedimiento("dbo.sp_DeleteSaleDetail", parameters);
                dtr = conn.EjecutarTableReader();

                while (dtr.Read())
                {
                    saleDetail = new SaleDetail
                    {
                        Id = int.Parse(dtr["SDId"].ToString()),
                        Product = new Product
                        {
                            Id = int.Parse(dtr["ProductId"].ToString()),
                            Name = dtr["ProductName"].ToString(),
                            UnitPrice = decimal.Parse(dtr["UnitPrice"].ToString())
                        },
                        Quantity = int.Parse(dtr["Qty"].ToString())
                    };
                }

                return Ok(saleDetail); // devuelve el detalle de venta eliminado
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Desconectar();
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace RestApi.Generado;

public partial class DetallesPedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int PlatoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace RestApi.Generado;

public partial class Pedido
{
    public int Id { get; set; }

    public int MesaId { get; set; }

    public DateTime Fecha { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();
}

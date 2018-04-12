using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SEP.Movil.Business.Hubs
{
    public class MesaDeTicketsHub : Hub
    {
        private readonly TicketManager _adminTickets;
        public MesaDeTicketsHub(TicketManager adminTickets)
        {
            _adminTickets = adminTickets;
        }
        public IEnumerable<Ticket> ObtieneTickets() 
        {
            return _adminTickets.ObtieneTickets();
        }
        public async Task CambiaEstadoTicket(string id, string operador, EstadoTicket estado)
        {
            await _adminTickets.BroadcastCambiaEstado(id, operador, estado);        
        }
        public async Task AgregaTicket()
        {
            await _adminTickets.BroadcastNuevoTicket();        
        }   
    }
}

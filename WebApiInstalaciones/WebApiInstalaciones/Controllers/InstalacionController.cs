using Entidades;
using Negocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;

namespace WebApiInstalaciones.Controllers
{

    [RoutePrefix("api/Resguardo")]
    public class InstalacionController : ApiController
    {

        private static string path = ConfigurationManager.AppSettings["uploadFile"];

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult GetLogin(Query q)
        {
            Usuario u = NegocioDao.GetOne(q);

            if (u != null)
            {
                if (u.mensaje == "Pass")
                    return BadRequest("Contraseña Incorrecta");
                else
                    return Ok(u);
            }
            else return BadRequest("Usuario no existe");
        }


        [HttpPost]
        [Route("Sync")]
        public IHttpActionResult GetSincronizar(Query q)
        {
            try
            {
                return Ok(NegocioDao.GetSync(q));
            }
            catch (Exception)
            {
                return BadRequest("No puedes Sincronizar");
            }
        }


        [HttpPost]
        [Route("SaveRegistro")]
        public IHttpActionResult SaveRegistro()
        {
            try
            {
                //string path = HttpContext.Current.Server.MapPath("~/Imagen/");
                var files = HttpContext.Current.Request.Files;
                var testValue = HttpContext.Current.Request.Form["data"];
                ParteDiario r = JsonConvert.DeserializeObject<ParteDiario>(testValue);
                Mensaje m = NegocioDao.SaveRegistro(r);
                if (m != null)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fileName = Path.GetFileName(files[i].FileName);
                        files[i].SaveAs(path + fileName);
                    }

                    return Ok(m);
                }
                else
                    return BadRequest("Error");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("SaveGps")]
        public IHttpActionResult SaveOperarioGps(EstadoOperario estadoOperario)
        {
            Mensaje mensaje = NegocioDao.SaveGps(estadoOperario);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");

        }

        [HttpPost]
        [Route("SaveMovil")]
        public IHttpActionResult SaveMovil(EstadoMovil e)
        {
            Mensaje mensaje = NegocioDao.SaveMovil(e);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");
        }

        // nuevo

        [HttpPost]
        [Route("SaveFile")]
        public IHttpActionResult SaveInspeccionesPhoto()
        {
            try
            {
                var files = HttpContext.Current.Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);

                    files[i].SaveAs(path + fileName);
                }

                return Ok("Enviado");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        [Route("SaveParteDiario")]
        public IHttpActionResult SaveParteDiario(ParteDiario p)
        {
            Mensaje mensaje = NegocioDao.SaveParteDiario(p);
            if (mensaje != null)
            {
                return Ok(mensaje);
            }
            else
                return BadRequest("Error de Envio");
        }

    }
}

using Entidades;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NegocioDao
    {
        private static string db = ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public static Usuario GetOne(Query q)
        {
            try
            {
                Usuario u = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_M_GetLogin", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = q.login;

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            u = new Usuario();
                            if (q.pass == dr.GetString(7).ToLower())
                            {
                                u.usuarioId = dr.GetInt32(0);
                                u.nroDoc = dr.GetString(1);
                                u.apellidos = dr.GetString(2);
                                u.nombres = dr.GetString(3);
                                u.email = dr.GetString(4);
                                u.tipoUsuarioId = dr.GetInt32(5);
                                u.perfilId = dr.GetInt32(6);
                                u.pass = dr.GetString(7);
                                u.estado = dr.GetInt32(8);
                                u.personalId = dr.GetInt32(9);
                                u.empresaId = dr.GetInt32(10);
                                u.nombreEmpresa = dr.GetString(11);
                                u.mensaje = "Go";

                                // Accesos
                                SqlCommand cmdA = cn.CreateCommand();
                                cmdA.CommandTimeout = 0;
                                cmdA.CommandType = CommandType.StoredProcedure;
                                cmdA.CommandText = "DSIGE_PROY_M_ListaMenus";
                                cmdA.Parameters.Add("@id_Usuario", SqlDbType.Int).Value = u.usuarioId;
                                SqlDataReader drV = cmdA.ExecuteReader();
                                if (drV.HasRows)
                                {
                                    List<Accesos> a = new List<Accesos>();
                                    while (drV.Read())
                                    {
                                        a.Add(new Accesos()
                                        {
                                            opcionId = drV.GetInt32(0),
                                            nombre = drV.GetString(1),
                                            usuarioId = drV.GetInt32(2)
                                        });
                                    }
                                    u.accesos = a;
                                }
                            }
                            else
                            {
                                u.mensaje = "Pass";
                            }
                        }
                    }
                    cn.Close();
                }
                return u;

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static string EncriptarClave(string cExpresion, bool bEncriptarCadena)
        {
            string cResult = "";
            string cPatron = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwwyz";
            string cEncrip = "^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï7485912360^çºªæÆöûÿø£Ø×ƒ¬½¼¡«»ÄÅÉêèï";


            if (bEncriptarCadena == true)
            {
                cResult = CHRTRAN(cExpresion, cPatron, cEncrip);
            }
            else
            {
                cResult = CHRTRAN(cExpresion, cEncrip, cPatron);
            }

            return cResult;

        }

        public static string CHRTRAN(string cExpresion, string cPatronBase, string cPatronReemplazo)
        {
            string cResult = "";

            int rgChar;
            int nPosReplace;

            for (rgChar = 1; rgChar <= Strings.Len(cExpresion); rgChar++)
            {
                nPosReplace = Strings.InStr(1, cPatronBase, Strings.Mid(cExpresion, rgChar, 1));

                if (nPosReplace == 0)
                {
                    nPosReplace = rgChar;
                    cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                }
                else
                {
                    if (nPosReplace > cPatronReemplazo.Length)
                    {
                        nPosReplace = rgChar;
                        cResult = cResult + Strings.Mid(cExpresion, nPosReplace, 1);
                    }
                    else
                    {
                        cResult = cResult + Strings.Mid(cPatronReemplazo, nPosReplace, 1);
                    }
                }
            }
            return cResult;
        }
        public static Sync GetSync(Query q)
        {
            try
            {
                Sync s = new Sync();

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_GetParteDiario";
                    cmd.Parameters.Add("@empresaId", SqlDbType.Int).Value = q.empresaId;
                    cmd.Parameters.Add("@usuarioId", SqlDbType.Int).Value = q.usuarioId;
                    //cmd.Parameters.Add("@id_Personal", SqlDbType.Int).Value = q.personalId;
                    SqlDataReader drV = cmd.ExecuteReader();
                    if (drV.HasRows)
                    {
                        List<ParteDiario> v = new List<ParteDiario>();
                        while (drV.Read())
                        {
                            ParteDiario p = new ParteDiario()
                            {
                                parteDiarioId = drV.GetInt32(0),
                                identity = drV.GetInt32(0),
                                empresaId = drV.GetInt32(1),
                                servicioId = drV.GetInt32(2),
                                fechaAsignacion = drV.GetDateTime(3).ToString("dd/MM/yyyy"),
                                fechaRegistro = drV.GetDateTime(4).ToString("dd/MM/yyyy"),
                                horaInicio = drV.GetString(5),
                                horaTermino = drV.GetString(6),
                                totalHoras = drV.GetString(7),
                                cantidadHoras = drV.GetDecimal(8),
                                precio = drV.GetDecimal(9),
                                totalSoles = drV.GetDecimal(10),
                                usuarioEfectivoPolicialId = drV.GetInt32(11),
                                personalCoordinarId = drV.GetInt32(12),
                                personalJefeCuadrillaId = drV.GetInt32(13),
                                lugarTrabajoPD = drV.GetString(14),
                                nroObraTD = drV.GetString(15),
                                observacionesPD = drV.GetString(16),
                                latitud = drV.GetString(17),
                                longitud = drV.GetString(18),
                                firmaEfectivoPolicial = drV.GetString(19),
                                firmaJefeCuadrilla = drV.GetString(20),
                                estadoId = drV.GetInt32(21),
                                nombreJefeCuadrilla = drV.GetString(22),
                                nombreCoordinador = drV.GetString(23),
                                nombreServicio = drV.GetString(24),
                                nombreEstado = drV.GetString(25),
                                incidencia = drV.GetString(26),
                                fechaInicioPD = drV.GetDateTime(27).ToString("dd/MM/yyyy"),
                                fechaFinPD = drV.GetDateTime(28).ToString("dd/MM/yyyy")
                            };

                            SqlCommand cmdF = con.CreateCommand();
                            cmdF.CommandTimeout = 0;
                            cmdF.CommandType = CommandType.StoredProcedure;
                            cmdF.CommandText = "DSIGE_PROY_M_GetParteDiarioPhoto";
                            cmdF.Parameters.Add("@parteDiarioId", SqlDbType.Int).Value = p.parteDiarioId;
                            SqlDataReader drF = cmdF.ExecuteReader();
                            if (drF.HasRows)
                            {
                                List<ParteDiarioPhoto> f = new List<ParteDiarioPhoto>();
                                while (drF.Read())
                                {
                                    f.Add(new ParteDiarioPhoto()
                                    {
                                        photoId = drF.GetInt32(0),
                                        identity = drF.GetInt32(0),
                                        parteDiarioId = drF.GetInt32(1),
                                        fotoUrl = drF.GetString(2),
                                        estado = drF.GetInt32(3)
                                    });
                                }
                                p.photos = f;
                            };
                            v.Add(p);
                        }
                        s.pds = v;
                    }


                    SqlCommand cmdE = con.CreateCommand();
                    cmdE.CommandTimeout = 0;
                    cmdE.CommandType = CommandType.StoredProcedure;
                    cmdE.CommandText = "DSIGE_PROY_M_GetEstados";
                    SqlDataReader drE = cmdE.ExecuteReader();
                    if (drE.HasRows)
                    {
                        List<Estado> e = new List<Estado>();
                        while (drE.Read())
                        {
                            e.Add(new Estado()
                            {
                                estadoId = drE.GetInt32(0),
                                abreviatura = drE.GetString(1)
                            });
                        }
                        s.estados = e;
                    }


                    SqlCommand cmdD = con.CreateCommand();
                    cmdD.CommandTimeout = 0;
                    cmdD.CommandType = CommandType.StoredProcedure;
                    cmdD.CommandText = "DSIGE_PROY_M_GetArea";
                    cmdD.Parameters.Add("@usuarioId", SqlDbType.Int).Value = q.usuarioId;
                    SqlDataReader drD = cmdD.ExecuteReader();
                    if (drD.HasRows)
                    {
                        List<Area> p = new List<Area>();
                        while (drD.Read())
                        {
                            p.Add(new Area()
                            {
                                areaId = drD.GetInt32(0),
                                nombreArea = drD.GetString(1),
                                estado = drD.GetInt32(2)
                            });
                        }
                        s.areas = p;
                    }

                    SqlCommand cmdT = con.CreateCommand();
                    cmdT.CommandTimeout = 0;
                    cmdT.CommandType = CommandType.StoredProcedure;
                    cmdT.CommandText = "DSIGE_PROY_M_GetCargo";
                    SqlDataReader drT = cmdT.ExecuteReader();
                    if (drT.HasRows)
                    {
                        List<Cargo> p = new List<Cargo>();
                        while (drT.Read())
                        {
                            p.Add(new Cargo()
                            {
                                cargoId = drT.GetInt32(0),
                                nombreCargo = drT.GetString(1),
                                estado = drT.GetInt32(2)
                            });
                        }
                        s.cargos = p;
                    }

                    SqlCommand cmdTD = con.CreateCommand();
                    cmdTD.CommandTimeout = 0;
                    cmdTD.CommandType = CommandType.StoredProcedure;
                    cmdTD.CommandText = "DSIGE_PROY_M_GetTipo";
                    SqlDataReader drTD = cmdTD.ExecuteReader();
                    if (drTD.HasRows)
                    {
                        List<TipoDocumento> p = new List<TipoDocumento>();
                        while (drTD.Read())
                        {
                            p.Add(new TipoDocumento()
                            {
                                tipoId = drTD.GetInt32(0),
                                descripcion = drTD.GetString(1)
                            });
                        }
                        s.tipoDocuments = p;
                    }

                    SqlCommand cmdS = con.CreateCommand();
                    cmdS.CommandTimeout = 0;
                    cmdS.CommandType = CommandType.StoredProcedure;
                    cmdS.CommandText = "DSIGE_PROY_M_GetPersonal";
                    SqlDataReader drS = cmdS.ExecuteReader();
                    if (drS.HasRows)
                    {
                        List<Personal> p = new List<Personal>();
                        while (drS.Read())
                        {
                            p.Add(new Personal()
                            {
                                personalId = drS.GetInt32(0),
                                empresaId = drS.GetInt32(1),
                                tipoDocId = drS.GetInt32(2),
                                nroDocumento = drS.GetString(3),
                                apellidos = drS.GetString(4),
                                nombre = drS.GetString(5),
                                cargoId = drS.GetInt32(6),
                                direccion = drS.GetString(7),
                                distritoId = drS.GetInt32(8),
                                estado = drS.GetInt32(9),
                                usuarioId = drS.GetInt32(10)
                            });
                        }
                        s.personals = p;
                    }

                    con.Close();
                }
                return s;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Mensaje SaveRegistro(ParteDiario t)
        {
            try
            {
                int coordinadorId = 0;
                int cuadrillaId = 0;

                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    foreach (var d in t.personals)
                    {
                        SqlCommand cmdD = con.CreateCommand();
                        cmdD.CommandTimeout = 0;
                        cmdD.CommandType = CommandType.StoredProcedure;
                        cmdD.CommandText = "DSIGE_PROY_M_SavePersonal";
                        cmdD.Parameters.Add("@personalId", SqlDbType.Int).Value = d.personalId;
                        cmdD.Parameters.Add("@id_empresa", SqlDbType.Int).Value = d.empresaId;
                        cmdD.Parameters.Add("@id_tipodoc", SqlDbType.Int).Value = d.tipoDocId;
                        cmdD.Parameters.Add("@nrodocumento_personal", SqlDbType.VarChar).Value = d.nroDocumento;
                        cmdD.Parameters.Add("@apellidos_personal", SqlDbType.VarChar).Value = d.apellidos;
                        cmdD.Parameters.Add("@nombres_personal", SqlDbType.VarChar).Value = d.nombre;
                        cmdD.Parameters.Add("@id_cargo", SqlDbType.Int).Value = d.cargoId;
                        cmdD.Parameters.Add("@direccion_personal", SqlDbType.VarChar).Value = d.direccion;
                        cmdD.Parameters.Add("@id_distrito", SqlDbType.Int).Value = d.distritoId;
                        cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                        cmdD.Parameters.Add("@usuario", SqlDbType.Int).Value = d.usuarioId;
                        SqlDataReader drD = cmdD.ExecuteReader();

                        if (drD.HasRows)
                        {
                            while (drD.Read())
                            {
                                if (d.cargoId == 5)
                                    coordinadorId = drD.GetInt32(0);
                                else
                                    cuadrillaId = drD.GetInt32(0);

                            }
                        }
                    }

                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SaveParteDiario";
                    cmdO.Parameters.Add("@parteDiarioId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@fechaasignacion_pd", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@fecharegistro_pd", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@horainicio_pd", SqlDbType.VarChar).Value = t.horaInicio;
                    cmdO.Parameters.Add("@horatermino_pd", SqlDbType.VarChar).Value = t.horaTermino;
                    cmdO.Parameters.Add("@totalhoras_pd", SqlDbType.VarChar).Value = t.totalHoras;
                    cmdO.Parameters.Add("@cantidadhoras_pd", SqlDbType.Decimal).Value = t.cantidadHoras;
                    cmdO.Parameters.Add("@precio_pd", SqlDbType.Decimal).Value = t.precio;
                    cmdO.Parameters.Add("@totalsoles_pd", SqlDbType.Decimal).Value = t.totalSoles;
                    cmdO.Parameters.Add("@id_usuarioefectivopolicial", SqlDbType.Int).Value = t.usuarioEfectivoPolicialId;
                    cmdO.Parameters.Add("@id_personalcoordinar", SqlDbType.Int).Value = coordinadorId == 0 ? t.personalCoordinarId : coordinadorId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = cuadrillaId == 0 ? t.personalJefeCuadrillaId : cuadrillaId;
                    cmdO.Parameters.Add("@lugartrabajo_pd", SqlDbType.VarChar).Value = t.lugarTrabajoPD;
                    cmdO.Parameters.Add("@nroobratd_pd", SqlDbType.VarChar).Value = t.nroObraTD;
                    cmdO.Parameters.Add("@observaciones_pd", SqlDbType.VarChar).Value = t.observacionesPD;
                    cmdO.Parameters.Add("@latitud_pd", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_pd", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@firma_efectivopolicial", SqlDbType.VarChar).Value = t.firmaEfectivoPolicial;
                    cmdO.Parameters.Add("@firma_jefecuadrilla", SqlDbType.VarChar).Value = t.firmaJefeCuadrilla;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuarioId", SqlDbType.Int).Value = t.usuarioId;
                    cmdO.Parameters.Add("@incidencia", SqlDbType.VarChar).Value = t.incidencia;
                    SqlDataReader drO = cmdO.ExecuteReader();
                                     
                    if (drO.HasRows)
                    {
                        m = new Mensaje();
                        while (drO.Read())
                        {
                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.parteDiarioId;

                            List<MensajeDetalle> de = new List<MensajeDetalle>();
                            foreach (var f in t.photos)
                            {
                                SqlCommand cmdF = con.CreateCommand();
                                cmdF.CommandTimeout = 0;
                                cmdF.CommandType = CommandType.StoredProcedure;
                                cmdF.CommandText = "DSIGE_PROY_M_SaveParteDiarioPhoto";
                                cmdF.Parameters.Add("@photoId", SqlDbType.Int).Value = f.identity;
                                cmdF.Parameters.Add("@id_partediario", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdF.Parameters.Add("@fotourl", SqlDbType.VarChar).Value = f.fotoUrl;
                                cmdF.Parameters.Add("@estado", SqlDbType.Int).Value = f.estado;
                                cmdF.Parameters.Add("@usuarioId", SqlDbType.Int).Value = t.usuarioId;
                                SqlDataReader drF = cmdF.ExecuteReader();

                                if (drF.HasRows)
                                {
                                    while (drF.Read())
                                    {
                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleBaseId = f.photoId,
                                            detalleRetornoId = drF.GetInt32(0)
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }

                    con.Close();

                    return m;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveGps(EstadoOperario e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoGps";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@latitud", SqlDbType.VarChar).Value = e.latitud;
                    cmd.Parameters.Add("@longitud", SqlDbType.VarChar).Value = e.longitud;
                    cmd.Parameters.Add("@fechaGPD", SqlDbType.VarChar).Value = e.fechaGPD;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }
                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Mensaje SaveMovil(EstadoMovil e)
        {
            try
            {
                Mensaje m = null;
                using (SqlConnection cn = new SqlConnection(db))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "DSIGE_PROY_M_EstadoMovil";
                    cmd.Parameters.Add("@operarioId", SqlDbType.Int).Value = e.operarioId;
                    cmd.Parameters.Add("@gpsActivo", SqlDbType.Int).Value = e.gpsActivo;
                    cmd.Parameters.Add("@estadoBateria", SqlDbType.Int).Value = e.estadoBateria;
                    cmd.Parameters.Add("@fecha", SqlDbType.VarChar).Value = e.fecha;
                    cmd.Parameters.Add("@modoAvion", SqlDbType.Int).Value = e.modoAvion;
                    cmd.Parameters.Add("@planDatos", SqlDbType.Int).Value = e.planDatos;

                    int a = cmd.ExecuteNonQuery();
                    if (a == 1)
                    {
                        m = new Mensaje
                        {
                            codigoBase = 1,
                            mensaje = "Enviado"
                        };
                    }

                    cn.Close();
                }
                return m;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // nuevo

        public static Mensaje SaveParteDiario(ParteDiario t)
        {
            try
            {
                int coordinadorId = 0;
                int cuadrillaId = 0;

                Mensaje m = null;

                using (SqlConnection con = new SqlConnection(db))
                {
                    con.Open();

                    foreach (var d in t.personals)
                    {
                        SqlCommand cmdD = con.CreateCommand();
                        cmdD.CommandTimeout = 0;
                        cmdD.CommandType = CommandType.StoredProcedure;
                        cmdD.CommandText = "DSIGE_PROY_M_SavePersonal";
                        cmdD.Parameters.Add("@personalId", SqlDbType.Int).Value = d.personalId;
                        cmdD.Parameters.Add("@id_empresa", SqlDbType.Int).Value = d.empresaId;
                        cmdD.Parameters.Add("@id_tipodoc", SqlDbType.Int).Value = d.tipoDocId;
                        cmdD.Parameters.Add("@nrodocumento_personal", SqlDbType.VarChar).Value = d.nroDocumento;
                        cmdD.Parameters.Add("@apellidos_personal", SqlDbType.VarChar).Value = d.apellidos;
                        cmdD.Parameters.Add("@nombres_personal", SqlDbType.VarChar).Value = d.nombre;
                        cmdD.Parameters.Add("@id_cargo", SqlDbType.Int).Value = d.cargoId;
                        cmdD.Parameters.Add("@direccion_personal", SqlDbType.VarChar).Value = d.direccion;
                        cmdD.Parameters.Add("@id_distrito", SqlDbType.Int).Value = d.distritoId;
                        cmdD.Parameters.Add("@estado", SqlDbType.Int).Value = d.estado;
                        cmdD.Parameters.Add("@usuario", SqlDbType.Int).Value = d.usuarioId;
                        SqlDataReader drD = cmdD.ExecuteReader();

                        if (drD.HasRows)
                        {
                            while (drD.Read())
                            {
                                if (d.cargoId == 5)
                                    coordinadorId = drD.GetInt32(0);
                                else
                                    cuadrillaId = drD.GetInt32(0);

                            }
                        }
                    }

                    SqlCommand cmdO = con.CreateCommand();
                    cmdO.CommandTimeout = 0;
                    cmdO.CommandType = CommandType.StoredProcedure;
                    cmdO.CommandText = "DSIGE_PROY_M_SaveParteDiario2";
                    cmdO.Parameters.Add("@parteDiarioId", SqlDbType.Int).Value = t.identity;
                    cmdO.Parameters.Add("@id_empresa", SqlDbType.Int).Value = t.empresaId;
                    cmdO.Parameters.Add("@id_servicios", SqlDbType.Int).Value = t.servicioId;
                    cmdO.Parameters.Add("@fechaasignacion_pd", SqlDbType.VarChar).Value = t.fechaAsignacion;
                    cmdO.Parameters.Add("@fecharegistro_pd", SqlDbType.VarChar).Value = t.fechaRegistro;
                    cmdO.Parameters.Add("@horainicio_pd", SqlDbType.VarChar).Value = t.horaInicio;
                    cmdO.Parameters.Add("@horatermino_pd", SqlDbType.VarChar).Value = t.horaTermino;
                    cmdO.Parameters.Add("@totalhoras_pd", SqlDbType.VarChar).Value = t.totalHoras;
                    cmdO.Parameters.Add("@cantidadhoras_pd", SqlDbType.Decimal).Value = t.cantidadHoras;
                    cmdO.Parameters.Add("@precio_pd", SqlDbType.Decimal).Value = t.precio;
                    cmdO.Parameters.Add("@totalsoles_pd", SqlDbType.Decimal).Value = t.totalSoles;
                    cmdO.Parameters.Add("@id_usuarioefectivopolicial", SqlDbType.Int).Value = t.usuarioEfectivoPolicialId;
                    cmdO.Parameters.Add("@id_personalcoordinar", SqlDbType.Int).Value = coordinadorId == 0 ? t.personalCoordinarId : coordinadorId;
                    cmdO.Parameters.Add("@id_personaljefecuadrilla", SqlDbType.Int).Value = cuadrillaId == 0 ? t.personalJefeCuadrillaId : cuadrillaId;
                    cmdO.Parameters.Add("@lugartrabajo_pd", SqlDbType.VarChar).Value = t.lugarTrabajoPD;
                    cmdO.Parameters.Add("@nroobratd_pd", SqlDbType.VarChar).Value = t.nroObraTD;
                    cmdO.Parameters.Add("@observaciones_pd", SqlDbType.VarChar).Value = t.observacionesPD;
                    cmdO.Parameters.Add("@latitud_pd", SqlDbType.VarChar).Value = t.latitud;
                    cmdO.Parameters.Add("@longitud_pd", SqlDbType.VarChar).Value = t.longitud;
                    cmdO.Parameters.Add("@firma_efectivopolicial", SqlDbType.VarChar).Value = t.firmaEfectivoPolicial;
                    cmdO.Parameters.Add("@firma_jefecuadrilla", SqlDbType.VarChar).Value = t.firmaJefeCuadrilla;
                    cmdO.Parameters.Add("@estado", SqlDbType.Int).Value = t.estadoId;
                    cmdO.Parameters.Add("@usuarioId", SqlDbType.Int).Value = t.usuarioId;
                    cmdO.Parameters.Add("@incidencia", SqlDbType.VarChar).Value = t.incidencia;
                    cmdO.Parameters.Add("@fechaInicioPD", SqlDbType.VarChar).Value = t.fechaInicioPD;
                    cmdO.Parameters.Add("@fechaFinPD", SqlDbType.VarChar).Value = t.fechaFinPD;
                    SqlDataReader drO = cmdO.ExecuteReader();

                    if (drO.HasRows)
                    {
                        m = new Mensaje();
                        while (drO.Read())
                        {
                            m.mensaje = "Enviado";
                            m.codigoRetorno = drO.GetInt32(0);
                            m.codigoBase = t.parteDiarioId;

                            List<MensajeDetalle> de = new List<MensajeDetalle>();
                            foreach (var f in t.photos)
                            {
                                SqlCommand cmdF = con.CreateCommand();
                                cmdF.CommandTimeout = 0;
                                cmdF.CommandType = CommandType.StoredProcedure;
                                cmdF.CommandText = "DSIGE_PROY_M_SaveParteDiarioPhoto";
                                cmdF.Parameters.Add("@photoId", SqlDbType.Int).Value = f.identity;
                                cmdF.Parameters.Add("@id_partediario", SqlDbType.Int).Value = m.codigoRetorno;
                                cmdF.Parameters.Add("@fotourl", SqlDbType.VarChar).Value = f.fotoUrl;
                                cmdF.Parameters.Add("@estado", SqlDbType.Int).Value = f.estado;
                                cmdF.Parameters.Add("@usuarioId", SqlDbType.Int).Value = t.usuarioId;
                                SqlDataReader drF = cmdF.ExecuteReader();

                                if (drF.HasRows)
                                {
                                    while (drF.Read())
                                    {
                                        de.Add(new MensajeDetalle()
                                        {
                                            detalleBaseId = f.photoId,
                                            detalleRetornoId = drF.GetInt32(0)
                                        });
                                    }
                                }
                            }
                            m.detalle = de;
                        }
                    }

                    con.Close();

                    return m;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
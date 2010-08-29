using System;
using GisSharpBlog.NetTopologySuite.IO;
using System.Diagnostics;
using GisSharpBlog.NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace osmuy
{
	class MainClass
	{
		const int NombreCalle = 0;
		const int CodigoDepartamento = 1;
		const int CodigoNombre = 4;
		const int Sentido = 5;
		
		const int Montevideo = 1;
		
		static Dictionary<string, string> Names = new Dictionary<string, string>()
		{
			{ "VAZQUEZ", "Vázquez"},
			{ "COPOLA", "Cópola"},
			{ "GUILLAMON", "Guillamón"},
			{ "ANDRES", "Andrés"},
			{ "JOAQUIN", "Joaquín"},
			{ "SUAREZ", "Suárez"},
			{ "ABAYUBA", "Abayubá"},
			{ "BAHIA", "Bahía"},
			{ "MARIA", "María"},
			{ "ESTACION", "Estación"},
			{ "MARTIN", "Martín"},
			{ "LIBANO", "Líbano"},
			{ "CONCEPCION", "Concepción"},
			{ "COLON", "Colón"},
			{ "NUMERO", "Número"},
			{ "CUALEGON", "Cualegón"},
			{ "PARAISO", "Paraíso"},
			{ "INGENIERIA", "Ingeniería"},
			{ "REPUBLICA", "República"},
			{ "ECONOMICAS", "Económicas"},
			{ "ADMINISTRACION", "Administración"},
			{ "AGRONOMIA", "Agronomía"},
			{ "GASTON", "Gastón"},
			{ "FUTBOL", "Fútbol"},
			{ "MALAGA", "Málaga"},
			{ "GERMAN", "Germán"},
			{ "QUIMICA", "Química"},
			{ "TECNICA", "Técnica"},
			{ "INDIGENA", "Indígena"},
			{ "HISTORICO", "Histórico"},
			{ "SOLIS", "Solís"},
			{ "RAMON", "Ramón"},
			{ "PITAGORAS", "Pitágoras"},
			{ "CASABO,", "Casabó,"},
			{ "TELEFONO", "Teléfono"},
			{ "PUBLICO", "público"},
			{ "OCEANOGRAFICO", "Oceanográfico"},
			{ "LYON", "Lyón"},
			{ "BATOVI", "Batoví"},
			{ "TABARE", "Tabaré"},
			{ "YAMANDU", "Yamandú"},
			{ "JOSE", "José"},
			{ "RODO", "Rodó"},
			{ "AMERICO", "Américo"},
			{ "CONVENCION", "Convención"},
			{ "RODRIGUEZ", "Rodríguez"},
			{ "DIAZ", "Díaz"},
			{ "GARCIA", "García"},
			{ "TOMAS", "Tomás"},
			{ "QUINTIN", "Quintín"},
			{ "FERNANDEZ", "Fernández"},
			{ "SANCHEZ", "Sánchez"},
			{ "TIMBO", "Timbó"},
			{ "SIMON", "Simón"},
			{ "BOLIVAR", "Bolívar"},
			{ "MARTI", "Martí"},
			{ "PEREZ", "Pérez"},
			{ "GOMEZ", "Gómez"},
			{ "PAYSANDU", "Paysandú"},
			{ "RINCON", "Rincón"},
			{ "YACARE", "Yacaré"},
			{ "GUARANI", "Guaraní"},
			{ "ITUZAINGO", "Ituzaingó"},
			{ "AMORIN", "Amorín"},
			{ "TACUAREMBO", "Tacuarembó"},
			{ "DEMOSTENES", "Demóstenes"},
			{ "TRISTAN", "Tristán"},
			{ "LIBER", "Líber"},
			{ "CONSTITUCION", "Constitución"},
			{ "CUFRE", "Cufré"},
			{ "ASUNCION", "Asunción"},
			{ "PANAMA", "Panamá"},
			{ "CESAR", "César"},
			{ "FE", "Fé"},
			{ "CIVICOS", "Cívicos"},
			{ "FARIAS", "Farías"},
			{ "CAPITAN", "Capitán"},
			{ "RIO", "Río"},
			{ "ORDOÑEZ", "Ordóñez"},
			{ "MILLAN", "Millán"},
			{ "AMERICA", "América"},
			{ "POLICIA", "Policía"},
			{ "BAUZA", "Bauzá"},
			{ "LEGUIZAMON", "Leguizamón"},
			{ "AMERICAS", "Américas"},
			{ "MARTINEZ", "Martínez"},
			{ "LOPEZ", "López"},
			{ "ESPINOLA", "Espínola"},
			{ "CONTINUACION", "Continuación"},
			{ "BARTOLOME", "Bartolomé"},
			{ "UNION", "Unión"},
			{ "ARBOLES", "árboles"},
			{ "PAJAROS", "Pájaros"},
			{ "HIPODROMO", "Hipódromo"},
			{ "SARANDI", "Sarandí"},
			{ "HAITI", "Haití"},
			{ "PUBLICA", "Pública"},
			{ "JAPON", "Japón"},
			{ "CANADA", "Canadá"},
			{ "PERU", "Perú"},
			{ "CEBOLLATI", "Cebollatí"},
			{ "YAGUARON", "Yaguarón"},
			{ "GARZON", "Garzón"},
			{ "IBIRAPITA", "Ibirapitá"},
			{ "CHAJA", "Chajá"},
			{ "JUPITER", "Júpiter"},
			{ "ALVAREZ", "Álvarez"},
			{ "ROSE", "Rosé"},
			{ "LAZARO", "Lázaro"},
			{ "FEDERACION", "Federación"},
			{ "MEDIODIA", "Mediodía"},
			{ "RIOS", "Ríos"},
			{ "ARQUIMEDES", "Arquímedes"},
			{ "OTORGUES", "Otorgués"},
			{ "ANASTASIO", "Anastásio"},
			{ "REDENCION", "Redención"},
			{ "POLVORIN", "polvorín"},
			{ "BERNABE", "Bernabé"},
			{ "EMANCIPACION", "Emancipación"},
			{ "ANGEL", "Ángel"},
			{ "RAMIREZ", "Ramírez"},
			{ "TURQUIA", "Turquía"},
			{ "BOGOTA", "Bogotá"},
			{ "BELGICA", "Bélgica"},
			{ "MEXICO", "México"},
			{ "MALVIN", "Malvín"},
			{ "CONCILIACION", "Conciliación"},
			{ "PINZON", "Pinzón"},
			{ "AGUSTIN", "Agustín"},
			{ "MARACANA", "Maracaná"},
			{ "SOFIA", "Sofía"},
			{ "SEBASTIAN", "Sebastián"},
			{ "OLIMPICO", "Olímpico"},
			{ "CAMAMBU", "Camambú"},
			{ "HUERFANAS", "Huérfanas"},
			{ "OMBU", "Ombú"},
			{ "LUCIA", "Lucía"},
			{ "ÑANGUIRU", "Ñanguirú"},
			{ "MANGORE", "Mangoré"},
			{ "ARAGON", "Aragón"},
			{ "CAAZAPA", "Caazapá"},
			{ "TANGARUPA", "Tangarupá"},
			{ "CARAPEGUA", "Carapeguá"},
			{ "VELODROMO", "Velódromo"},
			{ "GUZMAN", "Guzmán"},
			{ "GUAYRA", "Guayrá"},
			{ "QUINTIN)", "Quintín)"},
			{ "ASIS", "Asís"},
			{ "JESUS", "Jesús"},
			{ "FARAMIÑAN", "Faramiñán"},
			{ "JULIAN", "Julián"},
			{ "GUAZUCUA", "Guazucuá"},
			{ "BOQUERON", "Boquerón"},
			{ "TURUBI", "Turubí"},
			{ "ABIARU", "Abiarú"},
			{ "CAPIATA", "Capiatá"},
			{ "NEUQUEN", "Neuquén"},
			{ "TUCUMAN", "Tucumán"},
			{ "CORDOBA", "Córdoba"},
			{ "ITAPE", "Itapé"},
			{ "MARMOL", "Mármol"},
			{ "DIOGENES", "Diógenes"},
			{ "CAONABO", "Caonabó"},
			{ "AGUEDA", "Águeda"},
			{ "LEON", "León"},
			{ "CORDOBES", "Cordobés"},
			{ "GARRE", "Garré"},
			{ "VAZQUES", "Vázques"},
			{ "CADIZ", "Cádiz"},
			{ "YAPEYU", "Yapeyú"},
			{ "CATALA", "Catalá"},
			{ "MONICA", "Mónica"},
			{ "FAMAILLA", "Famaillá"},
			{ "INDIGENAS", "Indígenas"},
			{ "GUTIERREZ", "Gutiérrez"},
			{ "BLAS", "Blás"},
			{ "TRIAS", "Trías"},
			{ "GALAN", "Galán"},
			{ "FELIX", "Félix"},
			{ "CACERES", "Cáceres"},
			{ "ADRIAN", "Adrián"},
			{ "GALVAN", "Galván"},
			{ "BELTRAN", "Beltrán"},
			{ "MENDEZ", "Méndez"},
			{ "GONZALEZ", "González"},
			{ "PADRON", "Padrón"},
			{ "DURAN", "Durán"},
			{ "SAA", "Saá"},
			{ "AMEZAGA", "Amézaga"},
			{ "MARMARAJA", "Marmarajá"},
			{ "PATRON", "Patrón"},
			{ "YAGUARI", "Yaguarí"},
			{ "CUÑAPIRU", "Cuñapirú"},
			{ "EJERCITO", "Ejército"},
			{ "CARAGUATA", "Caraguatá"},
			{ "HEMOGENES", "Hemógenes"},
			{ "GUAVIYU", "Guaviyú"},
			{ "CARAPE", "Carapé"},
			{ "CAICOBE", "Caicobé"},
			{ "COMANDIYU", "Comandiyú"},
			{ "TRAPANI", "Trápani"},
			{ "GONZALES", "Gonzáles"},
			{ "TREBOL", "Trébol"},
			{ "TUYUTI", "Tuyutí"},
			{ "ZUBIRIA", "Zubiría"},
			{ "VICTOR", "Víctor"},
			{ "JARDIN", "Jardín"},
			{ "JAPONES", "Japonés"},
			{ "BOTANICO", "Botánico"},
			{ "MAXIMO", "Máximo"},
			{ "ROSALIA", "Rosalía"},
			{ "SALONICA", "Salónica"},
			{ "JUCUTUJA", "Jucutujá"},
			{ "MOLIERE", "Moliére"},
			{ "VELAZQUEZ", "Velázquez"},
			{ "MARQUEZ", "Márquez"},
			{ "DOMINGUEZ", "Domínguez"},
			{ "PANTALEON", "Pantaleón"},
			{ "GOYEN", "Goyén"},
			{ "CORCEGA", "Córcega"},
			{ "MEDITERRANEO", "Mediterráneo"},
			{ "RAUL", "Raúl"},
			{ "CAMARA", "Cámara"},
			{ "TECNOLOGICO", "Tecnológico"},
			{ "CUARAHI", "Cuarahí"},
			{ "PIRARAJA", "Pirarajá"},
			{ "GUAZUNAMBI", "Guazunambí"},
			{ "YUQUERI", "Yuquerí"},
			{ "TAMANDUA", "Tamanduá"},
			{ "TACUMBU", "Tacumbú"},
			{ "TIMON", "Timón"},
			{ "MATIAS", "Matías"},
			{ "CATOLICA", "Católica"},
			{ "DAMASO", "Dámaso"},
			{ "ARAZATI", "Arazatí"},
			{ "NUÑEZ", "Núñez"},
			{ "AVALOS", "Ávalos"},
			{ "ESTRAZULAS", "Estrázulas"},
			{ "ALMIRON", "Almirón"},
			{ "OMBUES", "Ombúes"},
			{ "PODESTA", "Podestá"},
			{ "JACARANDA", "Jacarandá"},
			{ "VIA", "Vía"},
			{ "ISOLICA", "Isólica"},
			{ "ETIOPIA", "Etiopía"},
			{ "CAMERUN", "Camerún"},
			{ "MARIN", "Marín"},
			{ "BALBIN", "Balbín"},
			{ "CORUMBE", "Corumbé"},
			{ "BERLIN", "Berlín"},
			{ "ALTANTICO", "Altántico"},
			{ "ALMERIA", "Almería"},
			{ "ACEGUA", "Aceguá"},
			{ "CORTES", "Cortés"},
			{ "SORIN", "Sorín"},
			{ "GURUYA", "Guruyá"},
			{ "ERRIA", "Erría"},
			
			{ "BV", "Bulevar" },
			{ "AV", "Avenida" },
			{ "GRAL", "General" },
			{ "CNO", "Camino" },
			{ "TTE", "Teniente" },
			{ "ARQ", "Arquitecto" },
			{ "DUQ", "Duque" },
			{ "FCO", "Francisco" },
			{ "PTE", "Presidente" },
			{ "PSJE", "Pasaje" },
			{ "PLA", "paralela" },
			{ "1RA", "primera" },
			{ "1ER", "primer" },
			{ "2DA", "segunda" },
			{ "3RA", "tercera" },
			{ "4TA", "cuarta" },
			{ "5TA", "quinta" },
			{ "BEL", "Belloni" },
			{ "PDRE", "Padre" },
			{ "BO", "Barrio" },
			{ "SEND", "Sendero" },
			{ "CAP", "Capitán" },
			{ "CONT", "Continuación" },
			{ "PROF", "Profesor" },
			{ "DR", "Doctor" },
			{ "PNAL", "Peatonal" },
			{ "ASENT", "Asentamiento" },
			{ "STA", "Santa" },
			{ "STO", "Santo" },
			{ "ING", "Ingeniero" },
			{ "MTRO", "Maestro" },
			{ "MTRA", "Maestra" },
			{ "GDOR", "Gobernador" },
			{ "MDRE", "Madre" },
			{ "CDTE", "Comandante" },
			{ "ALMTE", "Almirante" },
			{ "CNEL", "Coronel" },
			{ "AGRM", "Agrimensor" },
			{ "RBLA", "Rambla" },
			{ "MCAL", "Mariscal" },
		};
		
		static Dictionary<string, string> Exceptions = new Dictionary<string, string>
		{
			{ "FRANCISCO PLA", "Francisco Pla" },
		};
			
		static HashSet<string> LowerCase = new HashSet<string>
		{
			"Y",
			"DE",
			"DEL",
			"LA",
			"LOS",
			"LAS",
			"EL",
			"AL",
			"A",
		};
		
		class Way
		{
			public List<int> Nodes = new List<int>();
			public bool OneWay;
		}
		
		class Calle
		{
			public string Nombre;
			public List<Way> Ways = new List<Way>();
			
			public void Add(List<int> nodes, bool oneWay)
			{
				if(nodes.Count == 0) throw new ArgumentException();
				var reversed = new List<int>(nodes);
				reversed.Reverse();
				foreach(var way in Ways)
				{
					if(way.OneWay != oneWay) continue;
					if(Merge(way, nodes))
					{
						Merge(way);
						return;
					}
					if(!oneWay)
					{
						if(Merge(way, reversed))
						{
							Merge(way);
							return;
						}
					}
				}
				Ways.Add(new Way { Nodes = nodes, OneWay = oneWay });
			}
			
			static bool Merge(Way way, List<int> nodes)
			{
				if(way.Nodes.Last() == nodes.First())
				{
					way.Nodes.AddRange(nodes);
					return true;
				}
				else if(nodes.Last() == way.Nodes.First())
				{
					way.Nodes.InsertRange(0, nodes);
					return true;
				}
				return false;
			}
			
			void Merge(Way way)
			{
				var reversed = new List<int>(way.Nodes);
				reversed.Reverse();
				foreach(var other in Ways)
				{
					if(other == way || other.OneWay != way.OneWay) continue;
					if(Merge(other, way.Nodes))
					{
						Ways.Remove(way);
						return;
					}
					if(!way.OneWay)
					{
						if(Merge(other, reversed))
						{
							Ways.Remove(way);
							return;
						}
					}
				}
			}
		}

		public static void Main (string[] args)
		{
			ConvertCalles("../../../data/montevideo/v_vias_sentido", "montevideo", 
				(nombre, departamento) => 
			              departamento != Montevideo 
			              || nombre == "CALLE FICTICIA"
			              || nombre.Contains("PROY")
			              || nombre == "VIA FERREA");
//			ConvertCalles("../../../data/canelones/EjesCalles", "canelones", (n,d) => false);
		}
		
		
		private static void  ConvertCalles(string input, string output, Func<string, int, bool> filter)
		{
			var nodes = new Dictionary<Coordinate, int>();
			var calles = new Dictionary<int, Calle>();
			
			var vias = new ShapefileDataReader (input, new GeometryFactory ());
			while (vias.Read ()) 
			{
				var multiline = vias.Geometry as MultiLineString;
				if (multiline == null) throw new NotSupportedException ();
				var departamento = vias.GetInt32(CodigoDepartamento);
				var nombre = vias.GetString(NombreCalle);
				
				if(filter(nombre, departamento)) continue;
				
				var codigo = vias.GetInt32(CodigoNombre);
				var sentido = vias.GetInt32(Sentido);
				
				Calle calle;
				if(!calles.TryGetValue(codigo, out calle))
				{
					calle = new Calle { Nombre = nombre };
					calles[codigo] = calle;
				}
				
				foreach (var geometry in multiline) 
				{
					if (geometry == multiline) continue;
					var line = geometry as LineString;
					if (line == null) throw new NotSupportedException ();
					var wayNodes = new List<int>();
					foreach (Coordinate coord in line.Coordinates)
					{
						coord.X = (float)coord.X;
						coord.Y = (float)coord.Y;
						int node;
						if(!nodes.TryGetValue(coord, out node))
						{
							nodes[coord] = node = nodes.Count;
						}
						wayNodes.Add(node);
					}
					if (sentido < 0) wayNodes.Reverse();
					calle.Add(wayNodes, sentido != 0);
				}
			}
			
			SaveToOsm(nodes, calles, output + ".osm");
		}
		
		static void  SaveToOsm(Dictionary<Coordinate, int> nodes, Dictionary<int, Calle> calles, string path)
		{
			var trans = new CoordinateTransformationFactory().CreateFromCoordinateSystems(
				ProjectedCoordinateSystem.WGS84_UTM(21, false), 
				GeographicCoordinateSystem.WGS84).MathTransform;
			
			XmlDocument osmDoc = new XmlDocument();
			osmDoc.InsertBefore(osmDoc.CreateXmlDeclaration("1.0","utf-8",null), osmDoc.DocumentElement); 		
			var rootNode  = osmDoc.CreateElement("osm");
			rootNode.SetAttribute("version", "0.6");
		       osmDoc.AppendChild(rootNode);
		
			foreach(var node in nodes)
			{
				var nodeEl = osmDoc.CreateElement("node");
				rootNode.AppendChild(nodeEl);
				nodeEl.SetAttribute("id", OsmId(node.Value).ToString());
				nodeEl.SetAttribute("visible", "true");
				var latlon = trans.Transform(new[]{node.Key.X, node.Key.Y});
				nodeEl.SetAttribute("lat", latlon[1].ToString(CultureInfo.InvariantCulture));
				nodeEl.SetAttribute("lon", latlon[0].ToString(CultureInfo.InvariantCulture));
			}
		
			int wayId = nodes.Count;
			foreach(var calle in calles)
			{
				foreach(var way in calle.Value.Ways)
				{
					var wayEl = osmDoc.CreateElement("way");
					rootNode.AppendChild(wayEl);
					wayEl.SetAttribute("id", OsmId(wayId++).ToString());
					wayEl.SetAttribute("visible", "true");
					wayEl.SetAttribute("action", "modify");
					foreach(var node in way.Nodes)
					{
						var ndEl = osmDoc.CreateElement("nd");
						wayEl.AppendChild(ndEl);
						ndEl.SetAttribute("ref", OsmId(node).ToString());
					}
					var name = calle.Value.Nombre;
					AddTag(wayEl, "name", PrettifyName(name));
					AddTag(wayEl, "highway", name.Contains("PEATONAL") || name.Contains("PNAL")? "footway" : "residential");
					AddTag(wayEl, "source", "http://intgis.montevideo.gub.uy");
					AddTag(wayEl, "source:ref", calle.Key);
					if(way.OneWay)
					{
						AddTag(wayEl, "oneway", "yes");
					}
				}
			}
			osmDoc.Save(path);
		}
		
		static string PrettifyName(string name)
		{
			string exception;
			if(Exceptions.TryGetValue(name, out exception)) return exception;
			var words = name.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);
			var prettyName = new StringBuilder();
			for(int i=0; i<words.Length; ++i)
			{
				var word = words[i];
				string prettyWord;
				if(Names.TryGetValue(word, out prettyWord))
				{
					prettyName.Append(i == 0? Char.ToUpper(prettyWord[0]) : prettyWord[0]);
					prettyName.Append(prettyWord.Substring(1, prettyWord.Length-1));
				}
				else if(i != 0 && LowerCase.Contains(word))
				{
					prettyName.Append(word.ToLower());
				}
				else
				{
					prettyName.Append(word[0]);
					prettyName.Append(word.Substring(1, word.Length - 1).ToLower());
				}
				if(i < words.Length-1)
					prettyName.Append(" ");
			}
			return prettyName.ToString();
		}
		
		static void AddTag(XmlElement el, string key, object value)
		{
			var tag = el.OwnerDocument.CreateElement("tag");
			el.AppendChild(tag);
			tag.SetAttribute("k", key);
			tag.SetAttribute("v", value.ToString());
		}
		
		static int OsmId(int i)
		{
			return -i-1;
		}
	}
}


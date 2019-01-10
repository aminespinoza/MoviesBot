# MoviesBot

La idea del bot es que tengamos la capacidad de preguntar acerca de cada película basados en la API de OMDB

**http://www.omdbapi.com**

En el proyecto de "**moviepediabot**" vas a poder ver el funcionamiento básico del bot, cambio de idiomas y búsqueda de películas (hasta ahora).

En el nuevo proyecto, la versión 4.0, avancé en la persistencia de datos al evitar utilizar almacenamiento en memoria para el lenguaje seleccionado y me fui por table storage. La teoría dice que para un bot productivo lo mejor es hacer eso así que preferí ir por ahí.

La cuestión es que aún así debemos preservar ciertas cosas en el bot en memoria de caché y para ello son necesarios los [Accesores](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-v4-state?view=azure-bot-service-4.0&tabs=csharp), estos usan inyección de dependencias para poder almacenar información en el estado de la conversación y con ello tener algo de configuración.

Tengo [este](https://github.com/Azure/LearnAI-Bootcamp/blob/master/lab02.2-building_bots/1_Dialogs_and_Regex.md) enlace que he estado leyendo para aprender cómo hacerlo.

Otra parte en la que estoy detenido es acerca de los diálogos, la idea es poder conocer su clasificación y otras cosas más. Eso lo estoy aprendiendo porque ocupa un modo bien diferente a como era en la versión anterior.

Después de todo esto ya he aprendido a desplegar un bot en modo productivo usando contenedores con Kubernetes. Pienso que hacer eso usando Azure DevOps con IC/CD podremos verdaderamente dar un show.

¿Cómo lo ves? ¿Qué opinas?


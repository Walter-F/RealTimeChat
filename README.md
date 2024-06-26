# RealTimeChat

**RealTimeChat** — полноценное веб-приложение, включающее в себя реализацию чата в режиме реального времени на основе технологии SignalR.

**Frontend** составляющая была реализована с помощью библиотеки **React JS**, сочетая в себе следующие вспомогательные техологии: **Tailwind** — CSS-фреймворк, ориентированный на полезность, **Chakra UI** — библиотека на основе компонентов.

**Backend** составляющая была реализована на основе **ASP.NET Core Web API**, включая в себя технологию SignalR, которая позволяет с помощью веб-сокетов создавать приложения, работающие в реальном времени, и Redis для кэширования данных, необходимых для отправки сообщений между пользователями.
____

# Инструкция по запуску:
1. Для запуска Frontend составляющей необходимо в терминале перейти по пути Frontend -> realtimechat и прописать `npm run dev`
3. Для запуска BackEnd составляющей откройте проект через `.sln` файл и запустите его с помощью проекта docker-compose.

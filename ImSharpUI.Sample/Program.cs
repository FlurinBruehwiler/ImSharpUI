﻿using SkiaSharp;
using static SDL2.SDL;

if (SDL_Init(SDL_INIT_VIDEO) < 0)
{
    // Handle initialization error
    Console.WriteLine($"SDL_Init Error: {SDL_GetError()}");
    return;
}

// Create an SDL window
var window = SDL_CreateWindow("SDL2 C# OpenGL", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 800, 600, SDL_WindowFlags.SDL_WINDOW_OPENGL);
if (window == IntPtr.Zero)
{
    // Handle window creation error
    Console.WriteLine($"SDL_CreateWindow Error: {SDL_GetError()}");
    SDL_Quit();
    return;
}

var screenSurface = SDL_GetWindowSurface(window);


// Create an OpenGL context
var glContext = SDL_GL_CreateContext(window);
if (glContext == IntPtr.Zero)
{
    // Handle context creation error
    Console.WriteLine($"SDL_GL_CreateContext Error: {SDL_GetError()}");
    SDL_DestroyWindow(window);
    SDL_Quit();
    return;
}

// Make the OpenGL context current
SDL_GL_MakeCurrent(window, glContext);


var glInterface = GRGlInterface.CreateOpenGl(SDL_GL_GetProcAddress);
Console.WriteLine(glInterface.Validate());

// var context = GRContext.CreateGl(glInterface, new GRContextOptions
// {
//     AvoidStencilBuffers = true
// });
//
// var target = new GRBackendRenderTarget(800, 600, 0, 8, new GRGlFramebufferInfo());
//
// var surface = SKSurface.Create(context, target, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);

GRContext context = GRContext.Create(GRBackend.OpenGL, glInterface);
GRBackendRenderTargetDesc backendRenderTargetDescription = new GRBackendRenderTargetDesc
{
    Config = GRPixelConfig.Rgba8888,
    Height = 300,
    Width = 300,
    Origin = GRSurfaceOrigin.TopLeft,
    RenderTargetHandle = new IntPtr(0),
    SampleCount = 0,
    StencilBits = 8
};
var surface = SKSurface.Create(context, backendRenderTargetDescription);
var canvas = surface.Canvas;


// Main loop
var quit = false;
while (!quit)
{
    SDL_Event e;
    while (SDL_PollEvent(out e) != 0)
    {
        if (e.type == SDL_EventType.SDL_QUIT)
        {
            quit = true;
        }
    }

    canvas.DrawRect(100, 100, 10200, 200, new SKPaint
    {
        Color = SKColors.Red
    });

    // Swap the front and back buffers
    SDL_FillRect(screenSurface, IntPtr.Zero, 300);
    SDL_UpdateWindowSurface(window);
}

surface.Dispose();
// Clean up
SDL_GL_DeleteContext(glContext);
SDL_DestroyWindow(window);
SDL_Quit();
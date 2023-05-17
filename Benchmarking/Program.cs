// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Benchmarking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webenology.blazor.components;

BenchmarkRunner.Run(typeof(Program).Assembly);



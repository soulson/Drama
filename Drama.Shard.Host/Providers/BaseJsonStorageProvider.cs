﻿// this file is based on code from OrleansContrib/Orleans.Providers.MongoDB.
// it is licensed under the MIT license, which can be found in this directory.
// it has been modified from its original form.

using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Serialization;
using Orleans.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Drama.Shard.Host.Providers
{
	/// <summary>
	/// Base class for JSON-based grain storage providers.
	/// </summary>
	public abstract class BaseJsonStorageProvider : IStorageProvider
	{
		private JsonSerializerSettings serializerSettings;

		/// <summary>
		/// Logger object
		/// </summary>
		public Logger Log
		{
			get; protected set;
		}

		/// <summary>
		/// Storage provider name
		/// </summary>
		public string Name { get; protected set; }

		/// <summary>
		/// Data manager instance
		/// </summary>
		/// <remarks>The data manager is responsible for reading and writing JSON strings.</remarks>
		protected IJsonStateDataManager DataManager { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		protected BaseJsonStorageProvider()
		{
		}

		/// <summary>
		/// Initializes the storage provider.
		/// </summary>
		/// <param name="name">The name of this provider instance.</param>
		/// <param name="providerRuntime">A Orleans runtime object managing all storage providers.</param>
		/// <param name="config">Configuration info for this provider instance.</param>
		/// <returns>Completion promise for this operation.</returns>
		public virtual Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
		{
			Log = providerRuntime.GetLogger(GetType().FullName);
			var serializationManager = (SerializationManager)providerRuntime.ServiceProvider.GetService(typeof(SerializationManager));
			serializerSettings = OrleansJsonSerializer.GetDefaultSerializerSettings(serializationManager, providerRuntime.GrainFactory);
			serializerSettings.Formatting = config.GetBoolProperty(OrleansJsonSerializer.IndentJsonProperty, false) ? Formatting.Indented : Formatting.None;
			serializerSettings.TypeNameHandling = TypeNameHandling.None;
			return Task.CompletedTask;
		}

		/// <summary>
		/// Closes the storage provider during silo shutdown.
		/// </summary>
		/// <returns>Completion promise for this operation.</returns>
		public Task Close()
		{
			DataManager?.Dispose();
			DataManager = null;
			return Task.CompletedTask;
		}

		/// <summary>
		/// Reads persisted state from the backing store and deserializes it into the the target
		/// grain state object.
		/// </summary>
		/// <param name="grainType">A string holding the name of the grain class.</param>
		/// <param name="grainReference">Represents the long-lived identity of the grain.</param>
		/// <param name="grainState">A reference to an object to hold the persisted state of the grain.</param>
		/// <returns>Completion promise for this operation.</returns>
		public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			if (DataManager == null)
				throw new ArgumentException("DataManager property not initialized");

			var grainTypeName = grainType.Split('.').Last();

			var entityData = await DataManager.Read(grainTypeName, grainReference.ToKeyString());
			if (entityData != null)
			{
				ConvertFromStorageFormat(grainState, entityData);
			}
		}

		/// <summary>
		/// Writes the persisted state from a grain state object into its backing store.
		/// </summary>
		/// <param name="grainType">A string holding the name of the grain class.</param>
		/// <param name="grainReference">Represents the long-lived identity of the grain.</param>
		/// <param name="grainState">A reference to an object holding the persisted state of the grain.</param>
		/// <returns>Completion promise for this operation.</returns>
		public Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			if (DataManager == null)
				throw new ArgumentException("DataManager property not initialized");

			var grainTypeName = grainType.Split('.').Last();

			var entityData = ConvertToStorageFormat(grainState);
			return DataManager.Write(grainTypeName, grainReference.ToKeyString(), entityData);
		}

		/// <summary>
		/// Removes grain state from its backing store, if found.
		/// </summary>
		/// <param name="grainType">A string holding the name of the grain class.</param>
		/// <param name="grainReference">Represents the long-lived identity of the grain.</param>
		/// <param name="grainState">An object holding the persisted state of the grain.</param>
		/// <returns></returns>
		public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
		{
			if (DataManager == null)
				throw new ArgumentException("DataManager property not initialized");

			var grainTypeName = grainType.Split('.').Last();

			DataManager.Delete(grainTypeName, grainReference.ToKeyString());
			return Task.CompletedTask;
		}

		/// <summary>
		/// Serializes from a grain instance to a JSON document.
		/// </summary>
		/// <param name="grainState">Grain state to be converted into JSON storage format.</param>
		/// <remarks>
		/// See:
		/// http://msdn.microsoft.com/en-us/library/system.web.script.serialization.javascriptserializer.aspx
		/// for more on the JSON serializer.
		/// </remarks>
		protected string ConvertToStorageFormat(IGrainState grainState)
		{
			return JsonConvert.SerializeObject(grainState.State, serializerSettings);
		}

		/// <summary>
		/// Constructs a grain state instance by deserializing a JSON document.
		/// </summary>
		/// <param name="grainState">Grain state to be populated for storage.</param>
		/// <param name="entityData">JSON storage format representaiton of the grain state.</param>
		protected void ConvertFromStorageFormat(IGrainState grainState, string entityData)
		{
			JsonConvert.PopulateObject(entityData, grainState.State, serializerSettings);
		}
	}
}

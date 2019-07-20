/* Copyright 2019 Vivien Baguio.
 * Subject to the MIT License License.
 * See https://mit-license.org/
 */

 /// <summary>
 /// Fired if changes to the feature points were made:
 /// I.e. moved, removed, added
 /// </summary>
public class FeaturePointChanged { }

/// <summary>
/// Fired if you need the current vehicle to
/// destroy itself
/// </summary>
public class DestroyVehicleSignal { }

/// <summary>
/// Fired if the vehicle has done one lap
/// </summary>
public class LapSignal { }

/// <summary>
/// Fired if the vehicle respawned
/// </summary>
public class RespawnSignal { }

/// <summary>
/// Fired, when the track settings changed
/// </summary>
public class SettingsChangedSignal { }
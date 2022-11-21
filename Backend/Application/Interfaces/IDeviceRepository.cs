﻿using Domain;

namespace Application.Interfaces;

public interface IDeviceRepository
{
    #region CRUD

    // Create
    /// <summary>
    /// Adds a device to database
    /// </summary>
    /// <param name="device"></param>
    /// <returns>Added device</returns>
    Device AddDevice(Device device);

    // Read
    /// <summary>
    /// Gets all devices from database
    /// </summary>
    /// <returns>List of devices</returns>
    IEnumerable<Device> GetDevices();
    
    /// <summary>
    /// Gets the device with the given deviceId
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns>Device with the given serialNumber</returns>
    Device GetDevice(int deviceId);
    
    /// <summary>
    /// Gets the device with the given serialNumber
    /// </summary>
    /// <param name="serialNumber"></param>
    /// <returns>Device with the given serialNumber</returns>
    Device GetDevice(string serialNumber);

    // Update 
    /// <summary>
    /// Updates a device with the given deviceId
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="device"></param>
    /// <returns>The updated device</returns>
    Device UpdateDevice(int deviceId, Device device);

    // Delete 
    /// <summary>
    /// Deletes the device with the given deviceId
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns>Deleted device</returns>
    Device DeleteDevice(int deviceId);

    #endregion
    
    // Add user to device
    /// <summary>
    /// Adds an user to the given deviceId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    Device AddUserToDevice(int userId, int deviceId);
    
    // Remove user from device
    /// <summary>
    /// Deletes an user on given deviceId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    Device DeleteUserFromDevice(int userId, int deviceId);
    
    // Update user on device
    /// <summary>
    /// Updates an user on given deviceId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    /// <returns>Device with updated user</returns>
    Device UpdateUserOnDevice(int userId, int deviceId);

    /// <summary>
    /// Gets all assigned Devices from userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>List of assigned devices on user</returns>
    IEnumerable<Device> AssignedDevices(int userId);

    //Rebuild Database
    /// <summary>
    /// Rebuillds the database
    /// </summary>
    void RebuildDB();
}
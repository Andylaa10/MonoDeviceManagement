﻿using System.Runtime.InteropServices;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Domain;
using Moq;

namespace ServiceTestProject;

public class DeviceServiceTest
{
    // Member data
    
    #region ListOfDevicesWithUser

    public static IEnumerable<Object[]> ListOfDevicesWithUser_TestCase()
    {
        User user = new User
        {
            Id = 1, Email = "Kristian@mail.com", FirstName = "Kristian", LastName = "Hansen", Salt = "123123",
            Hash = "123123", Role = "Admin", WorkNumber = "12345678"
        };
        Device device1 = new Device { Id = 1, Amount = 1, DeviceName = "Seed device1", SerialNumber = "1234553", UserId = user.Id};
        Device device2 = new Device { Id = 2, Amount = 1, DeviceName = "Seed device2", SerialNumber = "1123", UserId = user.Id};
        Device device3 = new Device { Id = 3, Amount = 1, DeviceName = "Seed device3", SerialNumber = "54543", UserId = user.Id};

        yield return new Object[]
        {
            new Device[]
            {
            },
            new List<Device>()
        };
        
        yield return new object[]
        {
            new Device[]
            {
               device1
            },
            new List<Device>() { device1 }
        };
        
        yield return new object[]
        {
            new Device[]
            {
                device1,
                device2, 
                device3
            },
            new List<Device>() { device1, device2, device3 }
        };
    }

    #endregion
    
    #region GetAllDevicesTest

    public static IEnumerable<Object[]> GetAllDevices_TestCase()
    {
        Device device1 = new Device { Id = 1, Amount = 1, DeviceName = "Seed device1", SerialNumber = "1234553"};
        Device device2 = new Device { Id = 2, Amount = 1, DeviceName = "Seed device2", SerialNumber = "1123"};
        Device device3 = new Device { Id = 3, Amount = 1, DeviceName = "Seed device3", SerialNumber = "54543"};

        yield return new Object[]
        {
            new Device[]
            {
            },
            new List<Device>()
        };

        yield return new object[]
        {

            new Device[]
            {
                device1
            },
            new List<Device>() { device1 }
        };

        yield return new object[]
        {
            new Device[]
            {
                device1,
                device2, 
                device3
            },
            new List<Device>() { device1, device2, device3 }
        };
    }

    #endregion
    
    [Fact]
    public void CreateDeviceServiceTest()
    {
        // Arrange 
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();

        // Act 
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        // Assert
        Assert.NotNull(service);
        Assert.True(service is DeviceService);
    }
    
    [Theory]
    [MemberData(nameof(GetAllDevices_TestCase))]
    public void GetDevices(Device[] data, List<Device> expectedResult)
    {
        // Arrange
        var fakeRepo = data;
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        mockRepository.Setup(r => r.GetDevices()).Returns(fakeRepo);

        // Act 
        var actual = service.GetDevices();

        // Assert
        Assert.Equal(expectedResult, actual);
        Assert.True(Enumerable.SequenceEqual(expectedResult, actual));
        mockRepository.Verify(r => r.GetDevices(), Times.Once);
    }
    
    [Theory]
    [InlineData(1, 1)]      //Valid device
    [InlineData(2, 2)]      //Valid device
    public void GetValidDeviceTest(int deviceId, int expectedValueId)
    {
        // Arrange
        Device device1 = new Device { Id = deviceId, Amount = 1, DeviceName = "Seed device1", SerialNumber = "1234553"};
        Device device2 = new Device { Id = 3, Amount = 1, DeviceName = "Seed device2", SerialNumber = "1123"};
        
        var fakeRepo = new List<Device>();
        fakeRepo.Add(device1);
        fakeRepo.Add(device2);
        
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        mockRepository.Setup(r => r.GetDevice(deviceId)).Returns(fakeRepo.Find(d => d.Id == deviceId));
        
        // Act 
        var actual = service.GetDevice(deviceId);

        // Assert 
        Assert.Equal(expectedValueId, actual.Id);
        mockRepository.Verify(r => r.GetDevice(deviceId), Times.Once);
    }
    
    [Theory]
    [InlineData(0, "DeviceId cannot be less than 1 or null")]     //Invalid deviceId 0
    [InlineData(-1, "DeviceId cannot be less than 1 or null")]    //Invalid deviceId -1
    [InlineData(null, "DeviceId cannot be less than 1 or null")]  //Invalid deviceId null
    public void GetInvalidDeviceTest(int deviceId, string expectedMessage )
    {
        // Arrange
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        // Act 
        Action action = () => service.GetDevice(deviceId);  

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        Assert.Equal("DeviceId cannot be less than 1 or null", ex.Message);
        
        mockRepository.Verify(r => r.GetDevice(deviceId), Times.Never);
    }
    
    [Theory]
    [InlineData(1, "device1", 1, "123")]
    public void CreateValidDeviceTest(int deviceId, string deviceName, int amount, string serialNumber)
    {
        // Arrange
        List<Device> devices = new List<Device>();
        Device device = new Device{Id = deviceId, DeviceName = deviceName, Amount = amount, SerialNumber = serialNumber};
        PostDeviceDTO dto = new PostDeviceDTO { DeviceName = deviceName, Amount = amount, SerialNumber = serialNumber };
        
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        mockRepository.Setup(r => r.AddDevice(It.IsAny<Device>())).Returns(() =>
        {
            devices.Add(device);
            return device;
        });
        
        // Act 
        var createdDevice = service.AddDevice(dto);
        
        // Assert
        Assert.True(devices.Count == 1);
        Assert.Equal(device.Amount, createdDevice.Amount);
        Assert.Equal(device.Id, createdDevice.Id);
        Assert.Equal(device.DeviceName, createdDevice.DeviceName);
        Assert.Equal(device.SerialNumber, createdDevice.SerialNumber);
        mockRepository.Verify(r => r.AddDevice(It.IsAny<Device>()), Times.Once);
    }
    
    [Theory]
    [InlineData(1, "", "12345678", 1, "Device name cannot be empty or null")]                  //Invalid device with empty deviceName
    [InlineData(1, null, "12345678", 1, "Device name cannot be empty or null")]                //Invalid device with null as deviceName
    [InlineData(2, "Monitor", "", 1, "Device serialNumber cannot be empty or null")]           //Invalid device with empty deviceSerialNumber
    [InlineData(2, "Monitor", null, 1, "Device serialNumber cannot be empty or null")]         //Invalid device with empty deviceSerialNumber
    [InlineData(3, "iPad", "12345678", 0, "Device amount cannot be null or less than 0")]      //Invalid device with amount 0
    [InlineData(3, "iPad", "12345678", -1, "Device amount cannot be null or less than 0")]     //Invalid device with amount -1
    [InlineData(3, "iPad", "12345678", null, "Device amount cannot be null or less than 0" )]  //Invalid device with null as amount
    public void CreateInvalidDeviceTest(int deviceId, string deviceName, string serialNumber, int amount, string expectedMessage)
    {
        // Arrange
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        Device device = new Device{Id = deviceId, DeviceName = deviceName, Amount = amount, SerialNumber = serialNumber};
        PostDeviceDTO dto = new PostDeviceDTO { DeviceName = deviceName, Amount = amount, SerialNumber = serialNumber };
        // Act 
        var action = () => service.AddDevice(dto);

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r => r.AddDevice(device), Times.Never);
    }
    
    [Theory]
    [InlineData(1, "Laptop", "serialTest1", 1, "Device already exist")]                        //Invalid device that already exist
    [InlineData(1, "Laptop", "123464", 1, "Device already exist")]                             //Invalid device that already exist
    public void CreateExistingDeviceTest(int deviceId, string deviceName, string serialNumber, int amount, string expectedMessage)
    {
        // Arrange
        List<Device> devices = new List<Device>();
        Device existingDevice = new Device { Id = deviceId, DeviceName = deviceName, SerialNumber = serialNumber, Amount = amount };
        devices.Add(existingDevice);
        
        Device device = new Device{Id = existingDevice.Id, DeviceName = existingDevice.DeviceName, Amount = existingDevice.Amount, SerialNumber = existingDevice.SerialNumber};
        devices.Add(device);
        PostDeviceDTO dto = new PostDeviceDTO { DeviceName = device.DeviceName, Amount = device.Amount, SerialNumber = device.SerialNumber };

        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        mockRepository.Setup(r => r.AddDevice(It.IsAny<Device>())).Returns(() =>
        {
            devices.Add(device);
            return device;
        });
        
        // Act 
        var action = () => service.AddDevice(dto);

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r => r.AddDevice(It.IsAny<Device>()), Times.Never);
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData(1 , "Computer")]
    public void UpdateValidDeviceTest(int id, string deviceName)
    {
        // Arrange
        Device device = new Device{Id = id, DeviceName = "Laptop", Amount = 2, SerialNumber = "34"};
        PutDeviceDTO dto = new PutDeviceDTO {Id = device.Id, DeviceName = device.DeviceName, Amount = device.Amount, SerialNumber = device.SerialNumber };

        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PutDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        mockRepository.Setup(r => r.UpdateDevice(id, It.IsAny<Device>())).Returns(device);

        // Act 
        dto.DeviceName = deviceName;
        Device updateDevice = service.UpdateDevice(id, dto);

        // Assert
        Assert.Equal(device, updateDevice);
        Assert.Equal(device.Id, updateDevice.Id);
        Assert.Equal(device.SerialNumber, updateDevice.SerialNumber);
        Assert.Equal(device.DeviceName, updateDevice.DeviceName);
        Assert.Equal(device.Amount, updateDevice.Amount);
        mockRepository.Verify(r => r.UpdateDevice(id, It.IsAny<Device>()), Times.Once);
    }
    
    [Theory]
    [InlineData(0, "Laptop", "12345678", 1, "Device id cannot be null or less than 1")]        //Invalid device with id 0 
    [InlineData(-1, "Computer", "12345678", 1, "Device id cannot be null or less than 1")]     //Invalid device with id -1
    [InlineData(null, "Computer", "12345678", 1, "Device id cannot be null or less than 1")]   //Invalid device with null as id
    [InlineData(1, "", "12345678", 1, "Device name cannot be empty or null")]                  //Invalid device with empty deviceName
    [InlineData(1, null, "12345678", 1, "Device name cannot be empty or null")]                //Invalid device with null as deviceName
    [InlineData(2, "Monitor", "", 1, "Device serialNumber cannot be empty or null")]           //Invalid device with empty deviceSerialNumber
    [InlineData(2, "Monitor", null, 1, "Device serialNumber cannot be empty or null")]         //Invalid device with empty deviceSerialNumber
    [InlineData(3, "iPad", "12345678", 0, "Device amount cannot be null or less than 0")]      //Invalid device with amount 0
    [InlineData(3, "iPad", "12345678", -1, "Device amount cannot be null or less than 0")]     //Invalid device with amount -1
    [InlineData(3, "iPad", "12345678", null, "Device amount cannot be null or less than 0" )]  //Invalid device with null as amount
    public void InvalidDeviceUpdateTest(int deviceId, string deviceName, string deviceSerialNumber, int amount, string expectedMessage)
    {
        // Arrange
        Device device = new Device{Id = deviceId, DeviceName = deviceName, Amount = amount, SerialNumber = deviceSerialNumber};
        PutDeviceDTO dto = new PutDeviceDTO {Id = deviceId, DeviceName = deviceName, Amount = amount, SerialNumber = deviceSerialNumber};
        
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PutDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        
        // Act 
        var action = () => service.UpdateDevice(deviceId, dto);

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r=> r.UpdateDevice(deviceId, device),Times.Never);
    }

    //TODO update to existing device serial number 
    [Theory]
    [InlineData(2, "Id in the body and route are different")] //Invalid id not the same
    public void InvalidIdInputExceptionTest(int deviceId, string expectedMessage)
    {
        // Arrange
        Device device = new Device{Id = 1, DeviceName = "deviceName", Amount = 1, SerialNumber = "deviceSerialNumber"};
        PutDeviceDTO dto = new PutDeviceDTO {Id = 1, DeviceName = "deviceName", Amount = 1, SerialNumber = "deviceSerialNumber"};
        
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PutDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        
        // Act 
        var action = () => service.UpdateDevice(deviceId, dto);

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r=> r.UpdateDevice(deviceId, device),Times.Never);
    }

    [Theory]
    [InlineData(1, 1)] // Delete device with id 1 and expectedListSize 
    public void DeleteValidDeviceTest(int deviceId, int exceptedListSize)
    {
        // Arrange
        List<Device> devices = new List<Device>();
        Device deviceToDelete = new Device { Id = 1, Amount = 1, DeviceName = "Seed device1", SerialNumber = "1234553"};
        Device device2 = new Device { Id = 2, Amount = 1, DeviceName = "Seed device2", SerialNumber = "1123"};
        
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        mockRepository.Setup(r => r.GetDevices()).Returns(devices);
        mockRepository.Setup(r => r.DeleteDevice(deviceId)).Returns(() =>
        {
            devices.Remove(deviceToDelete);
            return deviceToDelete;
        });
        
        devices.Add(deviceToDelete);
        devices.Add(device2);
        
        // Act 
        var actual = service.DeleteDevice(deviceId);

        // Assert
        Assert.Equal(exceptedListSize, devices.Count);
        Assert.Equal(deviceToDelete, actual);
        Assert.DoesNotContain(deviceToDelete, devices);
        mockRepository.Verify(r=> r.DeleteDevice(deviceId), Times.Once);
    }
    
    [Theory]
    [InlineData(-1, "Device id cannot be null or less than 1")]   //Invalid deviceId -1
    [InlineData(0, "Device id cannot be null or less than 1")]    //Invalid deviceId 0
    [InlineData(null, "Device id cannot be null or less than 1")] //Invalid deviceId null
    public void DeleteInvalidDeviceTest(int deviceId, string expectedMessage)
    {
        // Arrange
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        // Act 
        var action = () => service.DeleteDevice(deviceId);

        // Assert
        var ex = Assert.Throws<ArgumentException>(action);
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r=>r.DeleteDevice(deviceId),Times.Never);
    }
    
    [Theory]
    [MemberData(nameof(ListOfDevicesWithUser_TestCase))]
    public void GetValidAssignedDevicesOnUserTest(Device[] data, List<Device> expectedResult)
    {
        var fakeRepo = data;
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);
        mockRepository.Setup(r => r.GetDevices()).Returns(fakeRepo);

        // Act 
        var actual = service.AssignedDevices(1);

        // Assert
        Assert.Equal(expectedResult, actual);
        Assert.True(Enumerable.SequenceEqual(expectedResult, actual));
        mockRepository.Verify(r => r.GetDevices(), Times.Once);
    }

    [Theory]
    [InlineData(null, "User id cannot be null or less than 1")] //User ID is null
    [InlineData(0, "User id cannot be null or less than 1")]    //User ID is 0
    [InlineData(-1, "User id cannot be null or less than 1")]   //User ID is -1
    public void GetInvalidAssignedDevicesOnUserTest(int userId, string expectedMessage)
    {
        Mock<IDeviceRepository> mockRepository = new Mock<IDeviceRepository>();
        var mapper = new MapperConfiguration(config =>
        {
            config.CreateMap<PostDeviceDTO, Device>();
        }).CreateMapper();
        var postDeviceValidator = new PostDeviceValidator();
        var putDeviceValidator = new PutDeviceValidator();
        
        IDeviceService service = new DeviceService(mockRepository.Object, mapper, postDeviceValidator, putDeviceValidator);

        // Act 
        var action = ()=> service.AssignedDevices(userId);
        var ex = Assert.Throws<ArgumentException>(action);

        // Assert
        Assert.Equal(expectedMessage, ex.Message);
        mockRepository.Verify(r => r.GetDevices(), Times.Never);
    }

    
    
    
    
    
}
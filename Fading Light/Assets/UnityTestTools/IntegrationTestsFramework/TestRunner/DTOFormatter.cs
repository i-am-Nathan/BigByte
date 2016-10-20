// file:	Assets\UnityTestTools\IntegrationTestsFramework\TestRunner\DTOFormatter.cs
//
// summary:	Implements the dto formatter class

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace UnityTest
{
    /// <summary>   A dto formatter. </summary>
    ///
 

    public class DTOFormatter {

        /// <summary>   Interface for transfer interface. </summary>
        ///
     

        private interface ITransferInterface
        {
            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref ResultDTO.MessageType val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref TestResultState val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref byte val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref bool val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref int val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref float val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref double val);

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            void Transfer(ref string val);
        }

        /// <summary>   A writer. </summary>
        ///
     

        private class Writer : ITransferInterface
        {
            /// <summary>   The stream. </summary>
            private readonly Stream _stream;

            /// <summary>   Constructor. </summary>
            ///
         
            ///
            /// <param name="stream">   The stream. </param>

            public Writer(Stream stream) { _stream = stream; }

            /// <summary>   Writes a converted number. </summary>
            ///
         
            ///
            /// <param name="bytes">    The bytes. </param>

            private void WriteConvertedNumber(byte[] bytes)
            {
                if(BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                _stream.Write(bytes, 0, bytes.Length);
            }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref ResultDTO.MessageType val) { _stream.WriteByte((byte)val); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref TestResultState val) { _stream.WriteByte((byte)val); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref byte val) { _stream.WriteByte(val); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref bool val) { _stream.WriteByte((byte)(val ? 0x01 : 0x00)); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref int val) { WriteConvertedNumber(BitConverter.GetBytes(val)); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref float val) { WriteConvertedNumber(BitConverter.GetBytes(val)); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref double val) { WriteConvertedNumber(BitConverter.GetBytes(val)); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref string val) 
            {
                var bytes = Encoding.BigEndianUnicode.GetBytes(val);
                int length = bytes.Length;
                Transfer(ref length);
                _stream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>   A reader. </summary>
        ///
     

        private class Reader : ITransferInterface
        {
            /// <summary>   The stream. </summary>
            private readonly Stream _stream;

            /// <summary>   Constructor. </summary>
            ///
         
            ///
            /// <param name="stream">   The stream. </param>

            public Reader(Stream stream) { _stream = stream; }

            /// <summary>   Reads converted number. </summary>
            ///
         
            ///
            /// <param name="size"> The size. </param>
            ///
            /// <returns>   An array of byte. </returns>

            private byte[] ReadConvertedNumber(int size)
            {
                byte[] buffer = new byte[size];
                _stream.Read (buffer, 0, buffer.Length);
                if(BitConverter.IsLittleEndian)
                    Array.Reverse(buffer);
                return buffer;
            }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref ResultDTO.MessageType val) { val = (ResultDTO.MessageType)_stream.ReadByte(); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref TestResultState val) { val = (TestResultState)_stream.ReadByte(); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref byte val) { val = (byte)_stream.ReadByte(); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref bool val) { val = (_stream.ReadByte() != 0); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref int val) { val = BitConverter.ToInt32(ReadConvertedNumber(4), 0); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref float val) { val = BitConverter.ToSingle(ReadConvertedNumber(4), 0); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref double val) { val = BitConverter.ToDouble(ReadConvertedNumber(8), 0); }

            /// <summary>   Transfers the given value. </summary>
            ///
         
            ///
            /// <param name="val">  [in,out] The value. </param>

            public void Transfer(ref string val) 
            {
                int length = 0;
                Transfer (ref length);
                var bytes = new byte[length];
                int remain = length;
                int index = 0;
                do {
                    int bytesRead = _stream.Read(bytes, index, remain);
                    remain -= bytesRead;
                    index += bytesRead;
                } while (remain > 0);
                val = Encoding.BigEndianUnicode.GetString(bytes);
            }
        }

        /// <summary>   Transfers. </summary>
        ///
     
        ///
        /// <param name="dto">      The dto. </param>
        /// <param name="transfer"> The transfer. </param>

        private void Transfer(ResultDTO dto, ITransferInterface transfer)
        {
            transfer.Transfer(ref dto.messageType);
            
            transfer.Transfer(ref dto.levelCount);
            transfer.Transfer(ref dto.loadedLevel);
            transfer.Transfer(ref dto.loadedLevelName);
            
            if(dto.messageType == ResultDTO.MessageType.Ping
               || dto.messageType == ResultDTO.MessageType.RunStarted
               || dto.messageType == ResultDTO.MessageType.RunFinished
               || dto.messageType == ResultDTO.MessageType.RunInterrupted
               || dto.messageType == ResultDTO.MessageType.AllScenesFinished)
                return;
                
            transfer.Transfer(ref dto.testName);
            transfer.Transfer(ref dto.testTimeout);
            
            if(dto.messageType == ResultDTO.MessageType.TestStarted)
                return;
            
            if(transfer is Reader)
                dto.testResult = new SerializableTestResult();
            SerializableTestResult str = (SerializableTestResult)dto.testResult;
            
            transfer.Transfer(ref str.resultState);
            transfer.Transfer(ref str.message);
            transfer.Transfer(ref str.executed);
            transfer.Transfer(ref str.name);
            transfer.Transfer(ref str.fullName);
            transfer.Transfer(ref str.id);
            transfer.Transfer(ref str.isSuccess);
            transfer.Transfer(ref str.duration);
            transfer.Transfer(ref str.stackTrace);
        }

        /// <summary>   Serialize this object to the given stream. </summary>
        ///
     
        ///
        /// <param name="stream">   The stream. </param>
        /// <param name="dto">      The dto. </param>

        public void Serialize (Stream stream, ResultDTO dto)
        {
            Transfer(dto, new Writer(stream));
        }

        /// <summary>   Deserialize this object to the given stream. </summary>
        ///
     
        ///
        /// <param name="stream">   The stream. </param>
        ///
        /// <returns>   An object. </returns>

        public object Deserialize (Stream stream)
        {
            var result = (ResultDTO)FormatterServices.GetSafeUninitializedObject(typeof(ResultDTO));
            Transfer (result, new Reader(stream));
            return result;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace RTS4.Common {
    // Summary:
    //     Defines a matrix.
    [Serializable]
    public struct XMatrix : IEquatable<XMatrix> {
        // Summary:
        //     Value at row 1 column 1 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M11;
        //
        // Summary:
        //     Value at row 1 column 2 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M12;
        //
        // Summary:
        //     Value at row 1 column 3 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M13;
        //
        // Summary:
        //     Value at row 1 column 4 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M14;
        //
        // Summary:
        //     Value at row 2 column 1 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M21;
        //
        // Summary:
        //     Value at row 2 column 2 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M22;
        //
        // Summary:
        //     Value at row 2 column 3 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M23;
        //
        // Summary:
        //     Value at row 2 column 4 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M24;
        //
        // Summary:
        //     Value at row 3 column 1 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M31;
        //
        // Summary:
        //     Value at row 3 column 2 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M32;
        //
        // Summary:
        //     Value at row 3 column 3 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M33;
        //
        // Summary:
        //     Value at row 3 column 4 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M34;
        //
        // Summary:
        //     Value at row 4 column 1 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M41;
        //
        // Summary:
        //     Value at row 4 column 2 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M42;
        //
        // Summary:
        //     Value at row 4 column 3 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M43;
        //
        // Summary:
        //     Value at row 4 column 4 of the matrix.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public XReal M44;

        //
        // Summary:
        //     Initializes a new instance of Matrix.
        //
        // Parameters:
        //   m11:
        //     Value to initialize m11 to.
        //
        //   m12:
        //     Value to initialize m12 to.
        //
        //   m13:
        //     Value to initialize m13 to.
        //
        //   m14:
        //     Value to initialize m14 to.
        //
        //   m21:
        //     Value to initialize m21 to.
        //
        //   m22:
        //     Value to initialize m22 to.
        //
        //   m23:
        //     Value to initialize m23 to.
        //
        //   m24:
        //     Value to initialize m24 to.
        //
        //   m31:
        //     Value to initialize m31 to.
        //
        //   m32:
        //     Value to initialize m32 to.
        //
        //   m33:
        //     Value to initialize m33 to.
        //
        //   m34:
        //     Value to initialize m34 to.
        //
        //   m41:
        //     Value to initialize m41 to.
        //
        //   m42:
        //     Value to initialize m42 to.
        //
        //   m43:
        //     Value to initialize m43 to.
        //
        //   m44:
        //     Value to initialize m44 to.
        [SuppressMessage("Microsoft.Design", "CA1025")]
        public XMatrix(XReal m11, XReal m12, XReal m13, XReal m14, XReal m21, XReal m22, XReal m23, XReal m24, XReal m31, XReal m32, XReal m33, XReal m34, XReal m41, XReal m42, XReal m43, XReal m44) {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        // Summary:
        //     Negates individual elements of a matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        public static XMatrix operator -(XMatrix m) {
            m.M11 = -m.M11; m.M12 = -m.M12; m.M13 = -m.M13; m.M13 = -m.M13;
            m.M21 = -m.M21; m.M22 = -m.M22; m.M23 = -m.M23; m.M23 = -m.M23;
            m.M31 = -m.M31; m.M32 = -m.M32; m.M33 = -m.M33; m.M33 = -m.M33;
            m.M41 = -m.M41; m.M42 = -m.M42; m.M43 = -m.M43; m.M43 = -m.M43;
            return m;
        }
        //
        // Summary:
        //     Subtracts matrices.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix operator -(XMatrix m1, XMatrix m2) {
            m1.M11 += -m2.M11; m1.M12 += -m2.M12; m1.M13 += -m2.M13; m1.M13 += -m2.M13;
            m1.M21 += -m2.M21; m1.M22 += -m2.M22; m1.M23 += -m2.M23; m1.M23 += -m2.M23;
            m1.M31 += -m2.M31; m1.M32 += -m2.M32; m1.M33 += -m2.M33; m1.M33 += -m2.M33;
            m1.M41 += -m2.M41; m1.M42 += -m2.M42; m1.M43 += -m2.M43; m1.M43 += -m2.M43;
            return m1;
        }
        //
        // Summary:
        //     Tests a matrix for inequality with another matrix.
        //
        // Parameters:
        //   matrix1:
        //     The matrix on the left of the equal sign.
        //
        //   matrix2:
        //     The matrix on the right of the equal sign.
        public static bool operator !=(XMatrix m1, XMatrix m2) {
            return m1.M11 != m2.M11 || m1.M12 != m2.M12 || m1.M13 != m2.M13 || m1.M14 != m2.M14 ||
                m1.M21 != m2.M21 || m1.M22 != m2.M22 || m1.M23 != m2.M23 || m1.M24 != m2.M24 ||
                m1.M31 != m2.M31 || m1.M32 != m2.M32 || m1.M33 != m2.M33 || m1.M34 != m2.M34 ||
                m1.M41 != m2.M41 || m1.M42 != m2.M42 || m1.M43 != m2.M43 || m1.M44 != m2.M44;
        }
        //
        // Summary:
        //     Multiplies a matrix by a scalar value.
        //
        // Parameters:
        //   scaleFactor:
        //     Scalar value.
        //
        //   matrix:
        //     Source matrix.
        public static XMatrix operator *(XReal scale, XMatrix m1) {
            m1.M11 *= scale; m1.M12 *= scale; m1.M13 *= scale; m1.M13 *= scale;
            m1.M21 *= scale; m1.M22 *= scale; m1.M23 *= scale; m1.M23 *= scale;
            m1.M31 *= scale; m1.M32 *= scale; m1.M33 *= scale; m1.M33 *= scale;
            m1.M41 *= scale; m1.M42 *= scale; m1.M43 *= scale; m1.M43 *= scale;
            return m1;
        }
        //
        // Summary:
        //     Multiplies a matrix by a scalar value.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        //
        //   scaleFactor:
        //     Scalar value.
        public static XMatrix operator *(XMatrix m1, XReal scale) {
            m1.M11 *= scale; m1.M12 *= scale; m1.M13 *= scale; m1.M13 *= scale;
            m1.M21 *= scale; m1.M22 *= scale; m1.M23 *= scale; m1.M23 *= scale;
            m1.M31 *= scale; m1.M32 *= scale; m1.M33 *= scale; m1.M33 *= scale;
            m1.M41 *= scale; m1.M42 *= scale; m1.M43 *= scale; m1.M43 *= scale;
            return m1;
        }
        //
        // Summary:
        //     Multiplies a matrix by another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix operator *(XMatrix m1, XMatrix m2) {
            throw new NotImplementedException();
            /*m1.M11 *= m2.M11; m1.M12 *= m2.M12; m1.M13 *= m2.M13; m1.M13 *= m2.M13;
            m1.M21 *= m2.M21; m1.M22 *= m2.M22; m1.M23 *= m2.M23; m1.M23 *= m2.M23;
            m1.M31 *= m2.M31; m1.M32 *= m2.M32; m1.M33 *= m2.M33; m1.M33 *= m2.M33;
            m1.M41 *= m2.M41; m1.M42 *= m2.M42; m1.M43 *= m2.M43; m1.M43 *= m2.M43;
            return m1;*/
        }
        //
        // Summary:
        //     Divides the components of a matrix by a scalar.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   divider:
        //     The divisor.
        public static XMatrix operator /(XMatrix m1, XReal divider) {
            XReal scale = 1 / divider;
            m1.M11 *= scale; m1.M12 *= scale; m1.M13 *= scale; m1.M13 *= scale;
            m1.M21 *= scale; m1.M22 *= scale; m1.M23 *= scale; m1.M23 *= scale;
            m1.M31 *= scale; m1.M32 *= scale; m1.M33 *= scale; m1.M33 *= scale;
            m1.M41 *= scale; m1.M42 *= scale; m1.M43 *= scale; m1.M43 *= scale;
            return m1;
        }
        //
        // Summary:
        //     Divides the components of a matrix by the corresponding components of another
        //     matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     The divisor.
        public static XMatrix operator /(XMatrix m1, XMatrix m2) {
            m1.M11 /= m2.M11; m1.M12 /= m2.M12; m1.M13 /= m2.M13; m1.M13 /= m2.M13;
            m1.M21 /= m2.M21; m1.M22 /= m2.M22; m1.M23 /= m2.M23; m1.M23 /= m2.M23;
            m1.M31 /= m2.M31; m1.M32 /= m2.M32; m1.M33 /= m2.M33; m1.M33 /= m2.M33;
            m1.M41 /= m2.M41; m1.M42 /= m2.M42; m1.M43 /= m2.M43; m1.M43 /= m2.M43;
            return m1;
        }
        //
        // Summary:
        //     Adds a matrix to another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix operator +(XMatrix m1, XMatrix m2) {
            m1.M11 += m2.M11; m1.M12 += m2.M12; m1.M13 += m2.M13; m1.M13 += m2.M13;
            m1.M21 += m2.M21; m1.M22 += m2.M22; m1.M23 += m2.M23; m1.M23 += m2.M23;
            m1.M31 += m2.M31; m1.M32 += m2.M32; m1.M33 += m2.M33; m1.M33 += m2.M33;
            m1.M41 += m2.M41; m1.M42 += m2.M42; m1.M43 += m2.M43; m1.M43 += m2.M43;
            return m1;
        }
        //
        // Summary:
        //     Compares a matrix for equality with another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static bool operator ==(XMatrix m1, XMatrix m2) {
            return m1.M11 == m2.M11 && m1.M12 == m2.M12 && m1.M13 == m2.M13 && m1.M14 == m2.M14 &&
                m1.M21 == m2.M21 && m1.M22 == m2.M22 && m1.M23 == m2.M23 && m1.M24 == m2.M24 &&
                m1.M31 == m2.M31 && m1.M32 == m2.M32 && m1.M33 == m2.M33 && m1.M34 == m2.M34 &&
                m1.M41 == m2.M41 && m1.M42 == m2.M42 && m1.M43 == m2.M43 && m1.M44 == m2.M44;
        }

        // Summary:
        //     Gets and sets the backward vector of the Matrix.
        public XVector3 Backward {
            get { return new XVector3(-M31, -M32, -M33); }
            set { M31 = -value.X; M32 = -value.Y; M33 = -value.Z; }
        }
        //
        // Summary:
        //     Gets and sets the down vector of the Matrix.
        public XVector3 Down {
            get { return new XVector3(-M21, -M22, -M23); }
            set { M21 = -value.X; M22 = -value.Y; M23 = -value.Z; }
        }
        //
        // Summary:
        //     Gets and sets the forward vector of the Matrix.
        public XVector3 Forward {
            get { return new XVector3(M31, M32, M33); }
            set { M31 = value.X; M32 = value.Y; M33 = value.Z; }
        }
        //
        // Summary:
        //     Returns an instance of the identity matrix.
        public static XMatrix Identity {
            get {
                return new XMatrix(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );
            }
        }
        //
        // Summary:
        //     Gets and sets the left vector of the Matrix.
        public XVector3 Left {
            get { return new XVector3(-M11, -M12, -M13); }
            set { M11 = -value.X; M12 = -value.Y; M13 = -value.Z; }
        }
        //
        // Summary:
        //     Gets and sets the right vector of the Matrix.
        public XVector3 Right {
            get { return new XVector3(M11, M12, M13); }
            set { M11 = value.X; M12 = value.Y; M13 = value.Z; }
        }
        //
        // Summary:
        //     Gets and sets the translation vector of the Matrix.
        public XVector3 Translation {
            get { return new XVector3(M41, M42, M43); }
            set { M41 = value.X; M42 = value.Y; M43 = value.Z; }
        }
        //
        // Summary:
        //     Gets and sets the up vector of the Matrix.
        public XVector3 Up {
            get { return new XVector3(M21, M22, M23); }
            set { M21 = value.X; M22 = value.Y; M23 = value.Z; }
        }

        // Summary:
        //     Adds a matrix to another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix Add(XMatrix matrix1, XMatrix matrix2) {
            return matrix1 + matrix2;
        }
        //
        // Summary:
        //     Adds a matrix to another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        //
        //   result:
        //     [OutAttribute] Resulting matrix.
        public static void Add(ref XMatrix m1, ref XMatrix m2, out XMatrix result) {
            result = m1 + m2;
        }
        //
        // Summary:
        //     Creates a spherical billboard that rotates around a specified object position.
        //
        // Parameters:
        //   objectPosition:
        //     Position of the object the billboard will rotate around.
        //
        //   cameraPosition:
        //     Position of the camera.
        //
        //   cameraUpVector:
        //     The up vector of the camera.
        //
        //   cameraForwardVector:
        //     Optional forward vector of the camera.
        public static XMatrix CreateBillboard(XVector3 objectPosition, XVector3 cameraPosition, XVector3 cameraUpVector, XVector3? cameraForwardVector) {
            XMatrix res; CreateBillboard(ref objectPosition, ref cameraPosition, ref cameraUpVector, cameraForwardVector, out res); return res;
        }
        //
        // Summary:
        //     Creates a spherical billboard that rotates around a specified object position.
        //
        // Parameters:
        //   objectPosition:
        //     Position of the object the billboard will rotate around.
        //
        //   cameraPosition:
        //     Position of the camera.
        //
        //   cameraUpVector:
        //     The up vector of the camera.
        //
        //   cameraForwardVector:
        //     Optional forward vector of the camera.
        //
        //   result:
        //     [OutAttribute] The created billboard matrix.
        public static void CreateBillboard(ref XVector3 objectPosition, ref XVector3 cameraPosition, ref XVector3 cameraUpVector, XVector3? cameraForwardVector, out XMatrix result) {
            XVector3 forward = cameraForwardVector ?? objectPosition - cameraPosition;
            CreateWorld(ref objectPosition, ref forward, ref cameraUpVector, out result);
        }
        //
        // Summary:
        //     Creates a cylindrical billboard that rotates around a specified axis.
        //
        // Parameters:
        //   objectPosition:
        //     Position of the object the billboard will rotate around.
        //
        //   cameraPosition:
        //     Position of the camera.
        //
        //   rotateAxis:
        //     Axis to rotate the billboard around.
        //
        //   cameraForwardVector:
        //     Optional forward vector of the camera.
        //
        //   objectForwardVector:
        //     Optional forward vector of the object.
        public static XMatrix CreateConstrainedBillboard(XVector3 objectPosition, XVector3 cameraPosition, XVector3 rotateAxis, XVector3? cameraForwardVector, XVector3? objectForwardVector) {
            XMatrix res; CreateConstrainedBillboard(ref objectPosition, ref cameraPosition, ref rotateAxis, cameraForwardVector, objectForwardVector, out res); return res;
        }
        //
        // Summary:
        //     Creates a cylindrical billboard that rotates around a specified axis.
        //
        // Parameters:
        //   objectPosition:
        //     Position of the object the billboard will rotate around.
        //
        //   cameraPosition:
        //     Position of the camera.
        //
        //   rotateAxis:
        //     Axis to rotate the billboard around.
        //
        //   cameraForwardVector:
        //     Optional forward vector of the camera.
        //
        //   objectForwardVector:
        //     Optional forward vector of the object.
        //
        //   result:
        //     [OutAttribute] The created billboard matrix.
        public static void CreateConstrainedBillboard(ref XVector3 objectPosition, ref XVector3 cameraPosition, ref XVector3 rotateAxis, XVector3? cameraForwardVector, XVector3? objectForwardVector, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Creates a new Matrix that rotates around an arbitrary vector.
        //
        // Parameters:
        //   axis:
        //     The axis to rotate around.
        //
        //   angle:
        //     The angle to rotate around the vector.
        public static XMatrix CreateFromAxisAngle(XVector3 axis, XReal angle) {
            XMatrix res; CreateFromAxisAngle(ref axis, angle, out res); return res;
        }
        //
        // Summary:
        //     Creates a new Matrix that rotates around an arbitrary vector.
        //
        // Parameters:
        //   axis:
        //     The axis to rotate around.
        //
        //   angle:
        //     The angle to rotate around the vector.
        //
        //   result:
        //     [OutAttribute] The created Matrix.
        public static void CreateFromAxisAngle(ref XVector3 axis, XReal angle, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Creates a rotation Matrix from a Quaternion.
        //
        // Parameters:
        //   quaternion:
        //     Quaternion to create the Matrix from.
        //public static Matrix CreateFromQuaternion(Quaternion quaternion);
        //
        // Summary:
        //     Creates a rotation Matrix from a Quaternion.
        //
        // Parameters:
        //   quaternion:
        //     Quaternion to create the Matrix from.
        //
        //   result:
        //     [OutAttribute] The created Matrix.
        //public static void CreateFromQuaternion(ref Quaternion quaternion, out Matrix result);
        //
        // Summary:
        //     Creates a new rotation matrix from a specified yaw, pitch, and roll.
        //
        // Parameters:
        //   yaw:
        //     Angle of rotation, in radians, around the y-axis.
        //
        //   pitch:
        //     Angle of rotation, in radians, around the x-axis.
        //
        //   roll:
        //     Angle of rotation, in radians, around the z-axis.
        public static XMatrix CreateFromYawPitchRoll(float yaw, float pitch, float roll) {
            XMatrix res; CreateFromYawPitchRoll(yaw, pitch, roll, out res); return res;
        }
        //
        // Summary:
        //     Fills in a rotation matrix from a specified yaw, pitch, and roll.
        //
        // Parameters:
        //   yaw:
        //     Angle of rotation, in radians, around the y-axis.
        //
        //   pitch:
        //     Angle of rotation, in radians, around the x-axis.
        //
        //   roll:
        //     Angle of rotation, in radians, around the z-axis.
        //
        //   result:
        //     [OutAttribute] An existing matrix filled in to represent the specified yaw,
        //     pitch, and roll.
        public static void CreateFromYawPitchRoll(float yaw, float pitch, float roll, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Creates a view matrix.
        //
        // Parameters:
        //   cameraPosition:
        //     The position of the camera.
        //
        //   cameraTarget:
        //     The target towards which the camera is pointing.
        //
        //   cameraUpVector:
        //     The direction that is "up" from the camera's point of view.
        public static XMatrix CreateLookAt(XVector3 cameraPosition, XVector3 cameraTarget, XVector3 cameraUpVector) {
            XMatrix res; CreateLookAt(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out res); return res;
        }
        //
        // Summary:
        //     Creates a view matrix.
        //
        // Parameters:
        //   cameraPosition:
        //     The position of the camera.
        //
        //   cameraTarget:
        //     The target towards which the camera is pointing.
        //
        //   cameraUpVector:
        //     The direction that is "up" from the camera's point of view.
        //
        //   result:
        //     [OutAttribute] The created view matrix.
        public static void CreateLookAt(ref XVector3 cameraPosition, ref XVector3 cameraTarget, ref XVector3 cameraUpVector, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds an orthogonal projection matrix.
        //
        // Parameters:
        //   width:
        //     Width of the view volume.
        //
        //   height:
        //     Height of the view volume.
        //
        //   zNearPlane:
        //     Minimum z-value of the view volume.
        //
        //   zFarPlane:
        //     Maximum z-value of the view volume.
        public static XMatrix CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane) {
            XMatrix res; CreateOrthographic(width, height, zNearPlane, zFarPlane, out res); return res;
        }
        //
        // Summary:
        //     Builds an orthogonal projection matrix.
        //
        // Parameters:
        //   width:
        //     Width of the view volume.
        //
        //   height:
        //     Height of the view volume.
        //
        //   zNearPlane:
        //     Minimum z-value of the view volume.
        //
        //   zFarPlane:
        //     Maximum z-value of the view volume.
        //
        //   result:
        //     [OutAttribute] The projection matrix.
        public static void CreateOrthographic(float width, float height, float zNearPlane, float zFarPlane, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds a customized, orthogonal projection matrix.
        //
        // Parameters:
        //   left:
        //     Minimum x-value of the view volume.
        //
        //   right:
        //     Maximum x-value of the view volume.
        //
        //   bottom:
        //     Minimum y-value of the view volume.
        //
        //   top:
        //     Maximum y-value of the view volume.
        //
        //   zNearPlane:
        //     Minimum z-value of the view volume.
        //
        //   zFarPlane:
        //     Maximum z-value of the view volume.
        public static XMatrix CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds a customized, orthogonal projection matrix.
        //
        // Parameters:
        //   left:
        //     Minimum x-value of the view volume.
        //
        //   right:
        //     Maximum x-value of the view volume.
        //
        //   bottom:
        //     Minimum y-value of the view volume.
        //
        //   top:
        //     Maximum y-value of the view volume.
        //
        //   zNearPlane:
        //     Minimum z-value of the view volume.
        //
        //   zFarPlane:
        //     Maximum z-value of the view volume.
        //
        //   result:
        //     [OutAttribute] The projection matrix.
        public static void CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds a perspective projection matrix and returns the result by value.
        //
        // Parameters:
        //   width:
        //     Width of the view volume at the near view plane.
        //
        //   height:
        //     Height of the view volume at the near view plane.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to the far view plane.
        public static XMatrix CreatePerspective(XReal width, XReal height, XReal nearPlaneDistance, XReal farPlaneDistance) {
            XMatrix res;
            CreatePerspectiveFieldOfView(width, height, nearPlaneDistance, farPlaneDistance, out res);
            return res;
        }
        //
        // Summary:
        //     Builds a perspective projection matrix and returns the result by reference.
        //
        // Parameters:
        //   width:
        //     Width of the view volume at the near view plane.
        //
        //   height:
        //     Height of the view volume at the near view plane.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to the far view plane.
        //
        //   result:
        //     [OutAttribute] The projection matrix.
        public static void CreatePerspective(XReal width, XReal height, XReal nearPlaneDistance, XReal farPlaneDistance, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds a perspective projection matrix based on a field of view and returns
        //     by value.
        //
        // Parameters:
        //   fieldOfView:
        //     Field of view in the y direction, in radians.
        //
        //   aspectRatio:
        //     Aspect ratio, defined as view space width divided by height. To match the
        //     aspect ratio of the viewport, the property AspectRatio.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to the far view plane.
        public static XMatrix CreatePerspectiveFieldOfView(XReal fieldOfView, XReal aspectRatio, XReal nearPlaneDistance, XReal farPlaneDistance) {
            XMatrix res;
            CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance, out res);
            return res;
        }
        //
        // Summary:
        //     Builds a perspective projection matrix based on a field of view and returns
        //     by reference.
        //
        // Parameters:
        //   fieldOfView:
        //     Field of view in the y direction, in radians.
        //
        //   aspectRatio:
        //     Aspect ratio, defined as view space width divided by height. To match the
        //     aspect ratio of the viewport, the property AspectRatio.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to the far view plane.
        //
        //   result:
        //     [OutAttribute] The perspective projection matrix.
        public static void CreatePerspectiveFieldOfView(XReal fieldOfView, XReal aspectRatio, XReal nearPlaneDistance, XReal farPlaneDistance, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Builds a customized, perspective projection matrix.
        //
        // Parameters:
        //   left:
        //     Minimum x-value of the view volume at the near view plane.
        //
        //   right:
        //     Maximum x-value of the view volume at the near view plane.
        //
        //   bottom:
        //     Minimum y-value of the view volume at the near view plane.
        //
        //   top:
        //     Maximum y-value of the view volume at the near view plane.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to of the far view plane.
        public static XMatrix CreatePerspectiveOffCenter(XReal left, XReal right, XReal bottom, XReal top, XReal nearPlaneDistance, XReal farPlaneDistance) {
            XMatrix res;
            CreatePerspectiveOffCenter(left, right, bottom, top, nearPlaneDistance, farPlaneDistance, out res);
            return res;
        }
        //
        // Summary:
        //     Builds a customized, perspective projection matrix.
        //
        // Parameters:
        //   left:
        //     Minimum x-value of the view volume at the near view plane.
        //
        //   right:
        //     Maximum x-value of the view volume at the near view plane.
        //
        //   bottom:
        //     Minimum y-value of the view volume at the near view plane.
        //
        //   top:
        //     Maximum y-value of the view volume at the near view plane.
        //
        //   nearPlaneDistance:
        //     Distance to the near view plane.
        //
        //   farPlaneDistance:
        //     Distance to of the far view plane.
        //
        //   result:
        //     [OutAttribute] The created projection matrix.
        public static void CreatePerspectiveOffCenter(XReal left, XReal right, XReal bottom, XReal top, XReal nearPlaneDistance, XReal farPlaneDistance, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Creates a Matrix that reflects the coordinate system about a specified Plane.
        //
        // Parameters:
        //   value:
        //     The Plane about which to create a reflection.
        //public static XMatrix CreateReflection(Plane value);
        //
        // Summary:
        //     Fills in an existing Matrix so that it reflects the coordinate system about
        //     a specified Plane.
        //
        // Parameters:
        //   value:
        //     The Plane about which to create a reflection.
        //
        //   result:
        //     [OutAttribute] A Matrix that creates the reflection.
        //public static void CreateReflection(ref Plane value, out Matrix result);
        //
        // Summary:
        //     Returns a matrix that can be used to rotate a set of vertices around the
        //     x-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the x-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        public static XMatrix CreateRotationX(XReal radians) {
            XMatrix res; CreateRotationX(radians, out res); return res;
        }
        //
        // Summary:
        //     Populates data into a user-specified matrix that can be used to rotate a
        //     set of vertices around the x-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the x-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        //
        //   result:
        //     [OutAttribute] The matrix in which to place the calculated data.
        public static void CreateRotationX(XReal radians, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Returns a matrix that can be used to rotate a set of vertices around the
        //     y-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the y-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        public static XMatrix CreateRotationY(XReal radians) {
            XMatrix res; CreateRotationY(radians, out res); return res;
        }
        //
        // Summary:
        //     Populates data into a user-specified matrix that can be used to rotate a
        //     set of vertices around the y-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the y-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        //
        //   result:
        //     [OutAttribute] The matrix in which to place the calculated data.
        public static void CreateRotationY(XReal radians, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Returns a matrix that can be used to rotate a set of vertices around the
        //     z-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the z-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        public static XMatrix CreateRotationZ(XReal radians) {
            XMatrix res; CreateRotationZ(radians, out res); return res;
        }
        //
        // Summary:
        //     Populates data into a user-specified matrix that can be used to rotate a
        //     set of vertices around the z-axis.
        //
        // Parameters:
        //   radians:
        //     The amount, in radians, in which to rotate around the z-axis. Note that you
        //     can use ToRadians to convert degrees to radians.
        //
        //   result:
        //     [OutAttribute] The rotation matrix.
        public static void CreateRotationZ(XReal radians, out XMatrix result) {
            throw new NotImplementedException();
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   scale:
        //     Amount to scale by.
        public static XMatrix CreateScale(XReal scale) {
            return CreateScale(scale, scale, scale);
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   scales:
        //     Amounts to scale by on the x, y, and z axes.
        public static XMatrix CreateScale(XVector3 scales) {
            return CreateScale(scales.X, scales.Y, scales.Z);
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   scale:
        //     Value to scale by.
        //
        //   result:
        //     [OutAttribute] The created scaling Matrix.
        public static void CreateScale(XReal scale, out XMatrix result) {
            CreateScale(scale, scale, scale, out result);
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   scales:
        //     Amounts to scale by on the x, y, and z axes.
        //
        //   result:
        //     [OutAttribute] The created scaling Matrix.
        public static void CreateScale(ref XVector3 scales, out XMatrix result) {
            CreateScale(scales.X, scales.Y, scales.Z, out result);
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   xScale:
        //     Value to scale by on the x-axis.
        //
        //   yScale:
        //     Value to scale by on the y-axis.
        //
        //   zScale:
        //     Value to scale by on the z-axis.
        public static XMatrix CreateScale(XReal xScale, XReal yScale, XReal zScale) {
            XMatrix scale;
            CreateScale(xScale, yScale, zScale, out scale);
            return scale;
        }
        //
        // Summary:
        //     Creates a scaling Matrix.
        //
        // Parameters:
        //   xScale:
        //     Value to scale by on the x-axis.
        //
        //   yScale:
        //     Value to scale by on the y-axis.
        //
        //   zScale:
        //     Value to scale by on the z-axis.
        //
        //   result:
        //     [OutAttribute] The created scaling Matrix.
        public static void CreateScale(XReal xScale, XReal yScale, XReal zScale, out XMatrix result) {
            result = new XMatrix(xScale, 0, 0, 0,
                0, yScale, 0, 0,
                0, 0, zScale, 0,
                0, 0, 0, 1);
        }
        //
        // Summary:
        //     Creates a Matrix that flattens geometry into a specified Plane as if casting
        //     a shadow from a specified light source.
        //
        // Parameters:
        //   lightDirection:
        //     A Vector3 specifying the direction from which the light that will cast the
        //     shadow is coming.
        //
        //   plane:
        //     The Plane onto which the new matrix should flatten geometry so as to cast
        //     a shadow.
        //public static Matrix CreateShadow(Vector3 lightDirection, Plane plane);
        //
        // Summary:
        //     Fills in a Matrix to flatten geometry into a specified Plane as if casting
        //     a shadow from a specified light source.
        //
        // Parameters:
        //   lightDirection:
        //     A Vector3 specifying the direction from which the light that will cast the
        //     shadow is coming.
        //
        //   plane:
        //     The Plane onto which the new matrix should flatten geometry so as to cast
        //     a shadow.
        //
        //   result:
        //     [OutAttribute] A Matrix that can be used to flatten geometry onto the specified
        //     plane from the specified direction.
        //public static void CreateShadow(ref XVector3 lightDirection, ref Plane plane, out XMatrix result);
        //
        // Summary:
        //     Creates a translation Matrix.
        //
        // Parameters:
        //   position:
        //     Amounts to translate by on the x, y, and z axes.
        public static XMatrix CreateTranslation(XVector3 position) {
            return new XMatrix(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                position.X, position.Y, position.Z, 1);
        }
        //
        // Summary:
        //     Creates a translation Matrix.
        //
        // Parameters:
        //   position:
        //     Amounts to translate by on the x, y, and z axes.
        //
        //   result:
        //     [OutAttribute] The created translation Matrix.
        public static void CreateTranslation(ref XVector3 position, out XMatrix result) {
            CreateTranslation(position.X, position.Y, position.Z, out result);
        }
        //
        // Summary:
        //     Creates a translation Matrix.
        //
        // Parameters:
        //   xPosition:
        //     Value to translate by on the x-axis.
        //
        //   yPosition:
        //     Value to translate by on the y-axis.
        //
        //   zPosition:
        //     Value to translate by on the z-axis.
        public static XMatrix CreateTranslation(XReal xPosition, XReal yPosition, XReal zPosition) {
            XMatrix res; CreateTranslation(xPosition, yPosition, zPosition, out res); return res;
        }
        //
        // Summary:
        //     Creates a translation Matrix.
        //
        // Parameters:
        //   xPosition:
        //     Value to translate by on the x-axis.
        //
        //   yPosition:
        //     Value to translate by on the y-axis.
        //
        //   zPosition:
        //     Value to translate by on the z-axis.
        //
        //   result:
        //     [OutAttribute] The created translation Matrix.
        public static void CreateTranslation(XReal xPosition, XReal yPosition, XReal zPosition, out XMatrix result) {
            result = new XMatrix(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                xPosition, yPosition, zPosition, 1);
        }
        //
        // Summary:
        //     Creates a world matrix with the specified parameters.
        //
        // Parameters:
        //   position:
        //     Position of the object. This value is used in translation operations.
        //
        //   forward:
        //     Forward direction of the object.
        //
        //   up:
        //     Upward direction of the object; usually [0, 1, 0].
        public static XMatrix CreateWorld(XVector3 position, XVector3 forward, XVector3 up) {
            XMatrix res; CreateWorld(ref position, ref forward, ref up, out res); return res;
        }
        //
        // Summary:
        //     Creates a world matrix with the specified parameters.
        //
        // Parameters:
        //   position:
        //     Position of the object. This value is used in translation operations.
        //
        //   forward:
        //     Forward direction of the object.
        //
        //   up:
        //     Upward direction of the object; usually [0, 1, 0].
        //
        //   result:
        //     [OutAttribute] The created world matrix.
        public static void CreateWorld(ref XVector3 position, ref XVector3 forward, ref XVector3 up, out XMatrix result) {
            forward.Normalize();
            up.Normalize();
            XVector3 right = XVector3.Cross(forward, up);
            result = new XMatrix(
                right.X, right.Y, right.Z, 0,
                up.X, up.Y, up.Z, 0,
                forward.X, forward.Y, forward.Z, 0,
                position.X, position.Y, position.Z, 1
            );
        }

        private static XReal det2x2(XReal a, XReal b, XReal c, XReal d) {
            return a * d - b * c;
        }

        private static XReal det3x3(XReal a1, XReal a2, XReal a3, XReal b1, XReal b2, XReal b3, XReal c1, XReal c2, XReal c3) {
            return a1 * det2x2(b2, b3, c2, c3) - b1 * det2x2(a2, a3, c2, c3) + c1 * det2x2(a2, a3, b2, b3);
        }

        //
        // Summary:
        //     Extracts the scalar, translation, and rotation components from a 3D scale/rotate/translate
        //     (SRT) Matrix. Reference page contains code sample.
        //
        // Parameters:
        //   scale:
        //     [OutAttribute] The scalar component of the transform matrix, expressed as
        //     a Vector3.
        //
        //   rotation:
        //     [OutAttribute] The rotation component of the transform matrix, expressed
        //     as a Quaternion.
        //
        //   translation:
        //     [OutAttribute] The translation component of the transform matrix, expressed
        //     as a Vector3.
        //public bool Decompose(out Vector3 scale, out Quaternion rotation, out Vector3 translation);
        //
        // Summary:
        //     Calculates the determinant of the matrix.
        public XReal Determinant() {
            return
                M11 * det3x3(M22, M23, M24, M32, M33, M34, M42, M43, M44) -
                    M21 * det3x3(M12, M13, M14, M32, M33, M34, M42, M43, M44) +
                    M31 * det3x3(M12, M13, M14, M22, M23, M24, M42, M43, M44) -
                    M41 * det3x3(M12, M13, M14, M22, M23, M24, M32, M33, M34);
            /*return (M11 + M22 + M33 + M44) + (M21 + M32 + M43 + M14) + (M31 + M42 + M13 + M14) + (M41 + M12 + M23 + M34)
                - (M11 + M42 + M33 + M24) - (M21 + M12 + M43 + M34) - (M31 + M22 + M13 + M44) - (M41 + M32 + M23 + M14);*/
        }
        //
        // Summary:
        //     Divides the components of a matrix by a scalar.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   divider:
        //     The divisor.
        public static XMatrix Divide(XMatrix matrix1, XReal divider) {
            return matrix1 / divider;
        }
        //
        // Summary:
        //     Divides the components of a matrix by the corresponding components of another
        //     matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     The divisor.
        public static XMatrix Divide(XMatrix matrix1, XMatrix matrix2) {
            return matrix1 / matrix2;
        }
        //
        // Summary:
        //     Divides the components of a matrix by a scalar.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   divider:
        //     The divisor.
        //
        //   result:
        //     [OutAttribute] Result of the division.
        public static void Divide(ref XMatrix matrix1, XReal divider, out XMatrix result) {
            result = matrix1 / divider;
        }
        //
        // Summary:
        //     Divides the components of a matrix by the corresponding components of another
        //     matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     The divisor.
        //
        //   result:
        //     [OutAttribute] Result of the division.
        public static void Divide(ref XMatrix matrix1, ref XMatrix matrix2, out XMatrix result) {
            result = matrix1 / matrix2;
        }
        //
        // Summary:
        //     Determines whether the specified Object is equal to the Matrix.
        //
        // Parameters:
        //   other:
        //     The Object to compare with the current Matrix.
        public bool Equals(XMatrix other) {
            return this == other;
        }
        //
        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     Object with which to make the comparison.
        public override bool Equals(object obj) {
            return obj is XMatrix && Equals((XMatrix)obj);
        }
        //
        // Summary:
        //     Gets the hash code of this object.
        public override int GetHashCode() {
            return M11.GetHashCode() ^ M22.GetHashCode() ^ M33.GetHashCode()
                ^ M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode();
        }
        //
        // Summary:
        //     Calculates the inverse of a matrix.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        public static XMatrix Invert(XMatrix matrix) {
            XMatrix res; Invert(ref matrix, out res); return res;
        }
        //
        // Summary:
        //     Calculates the inverse of a matrix.
        //
        // Parameters:
        //   matrix:
        //     The source matrix.
        //
        //   result:
        //     [OutAttribute] The inverse of matrix. The same matrix can be used for both
        //     arguments.
        public static void Invert(ref XMatrix m, out XMatrix result) {
            XReal det = m.Determinant();

            result.M11 = det3x3(m.M22, m.M23, m.M24, m.M32, m.M33, m.M34, m.M42, m.M43, m.M44) / det;
            result.M12 = -det3x3(m.M12, m.M13, m.M14, m.M32, m.M33, m.M34, m.M42, m.M43, m.M44) / det;
            result.M13 = det3x3(m.M12, m.M13, m.M14, m.M22, m.M23, m.M24, m.M42, m.M43, m.M44) / det;
            result.M14 = -det3x3(m.M12, m.M13, m.M14, m.M22, m.M23, m.M24, m.M32, m.M33, m.M34) / det;

            result.M21 = -det3x3(m.M21, m.M23, m.M24, m.M31, m.M33, m.M34, m.M41, m.M43, m.M44) / det;
            result.M22 = det3x3(m.M11, m.M13, m.M14, m.M31, m.M33, m.M34, m.M41, m.M43, m.M44) / det;
            result.M23 = -det3x3(m.M11, m.M13, m.M14, m.M21, m.M23, m.M24, m.M41, m.M43, m.M44) / det;
            result.M24 = det3x3(m.M11, m.M13, m.M14, m.M21, m.M23, m.M24, m.M31, m.M33, m.M34) / det;

            result.M31 = det3x3(m.M21, m.M22, m.M24, m.M31, m.M32, m.M34, m.M41, m.M42, m.M44) / det;
            result.M32 = -det3x3(m.M11, m.M12, m.M14, m.M31, m.M32, m.M34, m.M41, m.M42, m.M44) / det;
            result.M33 = det3x3(m.M11, m.M12, m.M14, m.M21, m.M22, m.M24, m.M41, m.M42, m.M44) / det;
            result.M34 = -det3x3(m.M11, m.M12, m.M14, m.M21, m.M22, m.M24, m.M31, m.M32, m.M34) / det;

            result.M41 = -det3x3(m.M21, m.M22, m.M23, m.M31, m.M32, m.M33, m.M41, m.M42, m.M43) / det;
            result.M42 = det3x3(m.M11, m.M12, m.M13, m.M31, m.M32, m.M33, m.M41, m.M42, m.M43) / det;
            result.M43 = -det3x3(m.M11, m.M12, m.M13, m.M21, m.M22, m.M23, m.M41, m.M42, m.M43) / det;
            result.M44 = det3x3(m.M11, m.M12, m.M13, m.M21, m.M22, m.M23, m.M31, m.M32, m.M33) / det;
        }
        //
        // Summary:
        //     Linearly interpolates between the corresponding values of two matrices.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        //
        //   amount:
        //     Interpolation value.
        public static XMatrix Lerp(XMatrix matrix1, XMatrix matrix2, XReal amount) {
            XMatrix res; Lerp(ref matrix1, ref matrix2, amount, out res); return res;
        }
        //
        // Summary:
        //     Linearly interpolates between the corresponding values of two matrices.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        //
        //   amount:
        //     Interpolation value.
        //
        //   result:
        //     [OutAttribute] Resulting matrix.
        public static void Lerp(ref XMatrix m1, ref XMatrix m2, XReal a, out XMatrix r) {
            r.M11 = m1.M11 + (m2.M11 - m1.M11) * a; r.M12 = m1.M12 + (m2.M12 - m1.M12) * a; r.M13 = m1.M13 + (m2.M13 - m1.M13) * a; r.M14 = m1.M14 + (m2.M14 - m1.M14) * a;
            r.M21 = m1.M21 + (m2.M21 - m1.M21) * a; r.M22 = m1.M22 + (m2.M22 - m1.M22) * a; r.M23 = m1.M23 + (m2.M23 - m1.M23) * a; r.M24 = m1.M24 + (m2.M24 - m1.M24) * a;
            r.M31 = m1.M31 + (m2.M31 - m1.M31) * a; r.M32 = m1.M32 + (m2.M32 - m1.M32) * a; r.M33 = m1.M33 + (m2.M33 - m1.M33) * a; r.M34 = m1.M34 + (m2.M34 - m1.M34) * a;
            r.M41 = m1.M41 + (m2.M41 - m1.M41) * a; r.M42 = m1.M42 + (m2.M42 - m1.M42) * a; r.M43 = m1.M43 + (m2.M43 - m1.M43) * a; r.M44 = m1.M44 + (m2.M44 - m1.M44) * a;
        }
        //
        // Summary:
        //     Multiplies a matrix by a scalar value.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   scaleFactor:
        //     Scalar value.
        public static XMatrix Multiply(XMatrix matrix1, XReal scaleFactor) {
            return matrix1 * scaleFactor;
        }
        //
        // Summary:
        //     Multiplies a matrix by another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix Multiply(XMatrix matrix1, XMatrix matrix2) {
            return matrix1 * matrix2;
        }
        //
        // Summary:
        //     Multiplies a matrix by a scalar value.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   scaleFactor:
        //     Scalar value.
        //
        //   result:
        //     [OutAttribute] The result of the multiplication.
        public static void Multiply(ref XMatrix matrix1, XReal scaleFactor, out XMatrix result) {
            result = matrix1 * scaleFactor;
        }
        //
        // Summary:
        //     Multiplies a matrix by another matrix.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        //
        //   result:
        //     [OutAttribute] Result of the multiplication.
        public static void Multiply(ref XMatrix m1, ref XMatrix m2, out XMatrix r) {
            r.M11 = m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31 + m1.M14 * m2.M41;
            r.M12 = m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32 + m1.M14 * m2.M42;
            r.M13 = m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33 + m1.M14 * m2.M43;
            r.M14 = m1.M11 * m2.M14 + m1.M12 * m2.M24 + m1.M13 * m2.M34 + m1.M14 * m2.M44;

            r.M21 = m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31 + m1.M24 * m2.M41;
            r.M22 = m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32 + m1.M24 * m2.M42;
            r.M23 = m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33 + m1.M24 * m2.M43;
            r.M24 = m1.M21 * m2.M14 + m1.M22 * m2.M24 + m1.M23 * m2.M34 + m1.M24 * m2.M44;

            r.M31 = m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31 + m1.M34 * m2.M41;
            r.M32 = m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32 + m1.M34 * m2.M42;
            r.M33 = m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33 + m1.M34 * m2.M43;
            r.M34 = m1.M31 * m2.M14 + m1.M32 * m2.M24 + m1.M33 * m2.M34 + m1.M34 * m2.M44;

            r.M41 = m1.M41 * m2.M11 + m1.M42 * m2.M21 + m1.M43 * m2.M31 + m1.M44 * m2.M41;
            r.M42 = m1.M41 * m2.M12 + m1.M42 * m2.M22 + m1.M43 * m2.M32 + m1.M44 * m2.M42;
            r.M43 = m1.M41 * m2.M13 + m1.M42 * m2.M23 + m1.M43 * m2.M33 + m1.M44 * m2.M43;
            r.M44 = m1.M41 * m2.M14 + m1.M42 * m2.M24 + m1.M43 * m2.M34 + m1.M44 * m2.M44;
        }
        //
        // Summary:
        //     Negates individual elements of a matrix.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        public static XMatrix Negate(XMatrix matrix) {
            return -matrix;
        }
        //
        // Summary:
        //     Negates individual elements of a matrix.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        //
        //   result:
        //     [OutAttribute] Negated matrix.
        public static void Negate(ref XMatrix matrix, out XMatrix result) {
            result = -matrix;
        }
        //
        // Summary:
        //     Subtracts matrices.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        public static XMatrix Subtract(XMatrix matrix1, XMatrix matrix2) {
            return matrix1 - matrix2;
        }
        //
        // Summary:
        //     Subtracts matrices.
        //
        // Parameters:
        //   matrix1:
        //     Source matrix.
        //
        //   matrix2:
        //     Source matrix.
        //
        //   result:
        //     [OutAttribute] Result of the subtraction.
        public static void Subtract(ref XMatrix matrix1, ref XMatrix matrix2, out XMatrix result) {
            result = matrix1 - matrix2;
        }
        //
        // Summary:
        //     Retrieves a string representation of the current object.
        public override string ToString() {
            return "{\n"
                + "\t[" + M11 + ", " + M12 + ", " + M13 + ", " + M14 + "]\n"
                + "\t[" + M21 + ", " + M22 + ", " + M23 + ", " + M24 + "]\n"
                + "\t[" + M31 + ", " + M32 + ", " + M33 + ", " + M34 + "]\n"
                + "\t[" + M41 + ", " + M42 + ", " + M43 + ", " + M44 + "]\n"
                + "}";
        }
        //
        // Summary:
        //     Transforms a Matrix by applying a Quaternion rotation.
        //
        // Parameters:
        //   value:
        //     The Matrix to transform.
        //
        //   rotation:
        //     The rotation to apply, expressed as a Quaternion.
        //public static XMatrix Transform(XMatrix value, Quaternion rotation);
        //
        // Summary:
        //     Transforms a Matrix by applying a Quaternion rotation.
        //
        // Parameters:
        //   value:
        //     The Matrix to transform.
        //
        //   rotation:
        //     The rotation to apply, expressed as a Quaternion.
        //
        //   result:
        //     [OutAttribute] An existing Matrix filled in with the result of the transform.
        //public static void Transform(ref XMatrix value, ref Quaternion rotation, out XMatrix result);
        //
        // Summary:
        //     Transposes the rows and columns of a matrix.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        public static XMatrix Transpose(XMatrix matrix) {
            XMatrix res; Transpose(ref matrix, out res); return res;
        }
        //
        // Summary:
        //     Transposes the rows and columns of a matrix.
        //
        // Parameters:
        //   matrix:
        //     Source matrix.
        //
        //   result:
        //     [OutAttribute] Transposed matrix.
        public static void Transpose(ref XMatrix m, out XMatrix r) {
            r.M11 = m.M11; r.M12 = m.M21; r.M13 = m.M31; r.M14 = m.M41;
            r.M21 = m.M12; r.M22 = m.M22; r.M23 = m.M32; r.M24 = m.M42;
            r.M31 = m.M13; r.M32 = m.M23; r.M33 = m.M33; r.M34 = m.M43;
            r.M41 = m.M14; r.M42 = m.M24; r.M43 = m.M34; r.M44 = m.M44;
        }
    }
}

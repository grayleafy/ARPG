using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpringSystem
{
    //阻尼弹簧系统
    public class Spring
    {
        static float M_PI = Mathf.Acos(-1);
        static float fast_negexp(float x)
        {
            return 1.0f / (1.0f + x + 0.48f * x * x + 0.235f * x * x * x);
        }

        static float lerp(float x, float y, float a)
        {
            return (1.0f - a) * x + a * y;
        }

        static float clamp(float x, float minimum, float maximum)
        {
            return x > maximum ? maximum : x < minimum ? minimum : x;
        }

        static float max(float x, float y)
        {
            return x > y ? x : y;
        }

        static float min(float x, float y)
        {
            return x < y ? x : y;
        }

        static float fabs(float x)
        {
            if (x >= 0) return x;
            return -x;
        }

        static float copysign(float x, float y)
        {
            x = fabs(x);
            if (y > 0) y = 1;
            else if (y < 0) y = -1;
            return x * y;
        }

        static float fast_atan(float x)
        {
            float z = fabs(x);
            float w = z > 1.0f ? 1.0f / z : z;
            float y = (M_PI / 4.0f) * w - w * (w - 1) * (0.2447f + 0.0663f * w);
            return copysign(z > 1.0f ? M_PI / 2.0f - y : y, x);
        }

        static float squaref(float x)
        {
            return x * x;
        }



        /// <summary>
        /// lerp过渡
        /// </summary>
        /// <param name="x"></param>
        /// <param name="g"></param>
        /// <param name="halflife"></param>
        /// <param name="dt"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static float Damper(float x, float g, float halflife, float dt, float eps = 1e-5f)
        {
            return lerp(x, g, 1.0f - fast_negexp((0.69314718056f * dt) / (halflife + eps)));
        }

        public static Vector3 Damper(Vector3 x, Vector3 g, float halflife, float dt, float eps = 1e-5f)
        {

            return new Vector3(lerp(x.x, g.x, 1.0f - fast_negexp((0.69314718056f * dt) / (halflife + eps))),
                               lerp(x.y, g.y, 1.0f - fast_negexp((0.69314718056f * dt) / (halflife + eps))),
                               lerp(x.z, g.z, 1.0f - fast_negexp((0.69314718056f * dt) / (halflife + eps))));
        }

        public static Quaternion Damper(Quaternion x, Quaternion g, float halflife, float dt, float eps = 1e-5f)
        {
            return Quaternion.Slerp(x, g, 1.0f - fast_negexp((0.69314718056f * dt) / (halflife + eps)));
        }

        static float damper_decay_exact(float x, float halflife, float dt, float eps = 1e-5f)
        {
            return x * fast_negexp((0.69314718056f * dt) / (halflife + eps));
        }




        static float halflife_to_damping(float halflife, float eps = 1e-5f)
        {
            return (4.0f * 0.69314718056f) / (halflife + eps);
        }

        static float damping_to_halflife(float damping, float eps = 1e-5f)
        {
            return (4.0f * 0.69314718056f) / (damping + eps);
        }

        static float frequency_to_stiffness(float frequency)
        {
            return squaref(2.0f * M_PI * frequency);
        }

        static float stiffness_to_frequency(float stiffness)
        {
            return Mathf.Sqrt(stiffness) / (2.0f * M_PI);
        }

        static float critical_halflife(float frequency)
        {
            return damping_to_halflife(Mathf.Sqrt(frequency_to_stiffness(frequency) * 4.0f));
        }

        static float critical_frequency(float halflife)
        {
            return stiffness_to_frequency(squaref(halflife_to_damping(halflife)) / 4.0f);
        }

        /// <summary>
        /// 阻尼弹簧，可调节频率
        /// </summary>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <param name="x_goal"></param>
        /// <param name="v_goal"></param>
        /// <param name="frequency"></param>
        /// <param name="halflife"></param>
        /// <param name="dt"></param>
        /// <param name="x_accumulated"></param>
        /// <param name="v_accumulated"></param>
        /// <param name="eps"></param>
        public static void SpringDamper(
            float x,
            float v,
            float x_goal,
            float v_goal,
            float frequency,
            float halflife,
            float dt,
            out float x_accumulated,
            out float v_accumulated,
            float eps = 1e-5f)
        {
            float g = x_goal;
            float q = v_goal;
            float s = frequency_to_stiffness(frequency);
            float d = halflife_to_damping(halflife);
            float c = g + (d * q) / (s + eps);
            float y = d / 2.0f;

            if (fabs(s - (d * d) / 4.0f) < eps) // Critically Damped
            {
                float j0 = x - c;
                float j1 = v + j0 * y;

                float eydt = fast_negexp(y * dt);

                x = j0 * eydt + dt * j1 * eydt + c;
                v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
            }
            else if (s - (d * d) / 4.0f > 0.0) // Under Damped
            {
                float w = Mathf.Sqrt(s - (d * d) / 4.0f);
                float j = Mathf.Sqrt(squaref(v + y * (x - c)) / (w * w + eps) + squaref(x - c));
                float p = fast_atan((v + (x - c) * y) / (-(x - c) * w + eps));

                j = (x - c) > 0.0f ? j : -j;

                float eydt = fast_negexp(y * dt);

                x = j * eydt * Mathf.Cos(w * dt + p) + c;
                v = -y * j * eydt * Mathf.Cos(w * dt + p) - w * j * eydt * Mathf.Sin(w * dt + p);
            }
            else if (s - (d * d) / 4.0f < 0.0) // Over Damped
            {
                float y0 = (d + Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
                float y1 = (d - Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
                float j1 = (c * y0 - x * y0 - v) / (y1 - y0);
                float j0 = x - j1 - c;

                float ey0dt = fast_negexp(y0 * dt);
                float ey1dt = fast_negexp(y1 * dt);

                x = j0 * ey0dt + j1 * ey1dt + c;
                v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
            }

            x_accumulated = x;
            v_accumulated = v;
        }



        static float damping_ratio_to_stiffness(float ratio, float damping)
        {
            return squaref(damping / (ratio * 2.0f));
        }

        static float damping_ratio_to_damping(float ratio, float stiffness)
        {
            return ratio * 2.0f * Mathf.Sqrt(stiffness);
        }

        /// <summary>
        /// 阻尼弹簧，可调节阻尼比
        /// </summary>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <param name="x_goal"></param>
        /// <param name="v_goal"></param>
        /// <param name="damping_ratio"></param>
        /// <param name="halflife"></param>
        /// <param name="dt"></param>
        /// <param name="x_accumulated"></param>
        /// <param name="v_accumulated"></param>
        /// <param name="eps"></param>
        public static void SpringDamperByRatio(
            float x,
            float v,
            float x_goal,
            float v_goal,
            float damping_ratio,
            float halflife,
            float dt,
            out float x_accumulated,
            out float v_accumulated,
            float eps = 1e-5f)
        {
            float g = x_goal;
            float q = v_goal;
            float d = halflife_to_damping(halflife);
            float s = damping_ratio_to_stiffness(damping_ratio, d);
            float c = g + (d * q) / (s + eps);
            float y = d / 2.0f;

            if (fabs(s - (d * d) / 4.0f) < eps) // Critically Damped
            {
                float j0 = x - c;
                float j1 = v + j0 * y;

                float eydt = fast_negexp(y * dt);

                x = j0 * eydt + dt * j1 * eydt + c;
                v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
            }
            else if (s - (d * d) / 4.0f > 0.0) // Under Damped
            {
                float w = Mathf.Sqrt(s - (d * d) / 4.0f);
                float j = Mathf.Sqrt(squaref(v + y * (x - c)) / (w * w + eps) + squaref(x - c));
                float p = fast_atan((v + (x - c) * y) / (-(x - c) * w + eps));

                j = (x - c) > 0.0f ? j : -j;

                float eydt = fast_negexp(y * dt);

                x = j * eydt * Mathf.Cos(w * dt + p) + c;
                v = -y * j * eydt * Mathf.Cos(w * dt + p) - w * j * eydt * Mathf.Sin(w * dt + p);
            }
            else if (s - (d * d) / 4.0f < 0.0) // Over Damped
            {
                float y0 = (d + Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
                float y1 = (d - Mathf.Sqrt(d * d - 4 * s)) / 2.0f;
                float j1 = (c * y0 - x * y0 - v) / (y1 - y0);
                float j0 = x - j1 - c;

                float ey0dt = fast_negexp(y0 * dt);
                float ey1dt = fast_negexp(y1 * dt);

                x = j0 * ey0dt + j1 * ey1dt + c;
                v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
            }
            x_accumulated = x;
            v_accumulated = v;
        }


        static Vector3 quat_log(Quaternion q, float eps = 1e-8f)
        {
            float length = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);

            if (length < eps)
            {
                return new Vector3(q.x, q.y, q.z);
            }
            else
            {
                float halfangle = Mathf.Acos(Mathf.Clamp(q.w, -1.0f, 1.0f));
                return halfangle * (new Vector3(q.x, q.y, q.z) / length);
            }
        }

        static Quaternion quat_exp(Vector3 v, float eps = 1e-8f)
        {
            float halfangle = Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z);

            if (halfangle < eps)
            {
                return (new Quaternion(v.x, v.y, v.z, 1.0f)).normalized;
            }
            else
            {
                float c = Mathf.Cos(halfangle);
                float s = Mathf.Sin(halfangle) / halfangle;
                return new Quaternion(s * v.x, s * v.y, s * v.z, c);
            }
        }

        static Vector3 quat_to_scaled_angle_axis(Quaternion q, float eps = 1e-8f)
        {
            return 2.0f * quat_log(q, eps);
        }



        static Quaternion quat_from_scaled_angle_axis(Vector3 v, float eps = 1e-8f)
        {
            return quat_exp(v / 2.0f, eps);
        }

        /// <summary>
        /// 四元数阻尼弹簧过渡
        /// </summary>
        /// <param name="x"></param>
        /// <param name="v"></param>
        /// <param name="x_goal"></param>
        /// <param name="halflife"></param>
        /// <param name="dt"></param>
        public static void SpringDamper(
            Quaternion x,
            Vector3 v,
            Quaternion x_goal,
            float halflife,
            float dt,
            out Quaternion x_accumulated,
            out Vector3 v_accumulated)
        {
            float y = halflife_to_damping(halflife) / 2.0f;

            Vector3 j0 = quat_to_scaled_angle_axis((x * Quaternion.Inverse(x_goal)));
            Vector3 j1 = v + j0 * y;

            float eydt = fast_negexp(y * dt);

            x = quat_from_scaled_angle_axis(eydt * (j0 + j1 * dt)) * x_goal;
            v = eydt * (v - j1 * y * dt);
            x_accumulated = x;
            v_accumulated = v;
        }


        public static void TrackingSpringUpdateNoVelocityAcceleration(
            ref float x,
            ref float v,
            float x_goal,
            float x_gain,
            float dt)
        {
            v = lerp(v, (x_goal - x) / dt, x_gain);
            x = x + dt * v;
        }
    }
}



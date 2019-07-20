/* Copyright 2019 Vivien Baguio.
 * Subject to the GNU General Public License.
 * See https://mit-license.org/
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

/// <summary>
/// Drop the corresponding prefab into the scene to check
/// if the authentication flow works
/// </summary>
public class AuthTester : MonoBehaviour
{
    public Image m_authIndicatorImage;

    private static Color POSITIVE = Color.green;
    private static Color NEGATIVE = Color.red;

    [Inject]
    private void Setup(AuthApi auth)
    {
        if (m_authIndicatorImage == null) {
            Debug.LogWarning ("The tester only works, if you provide an auth indicator image");
            return;
        }

        m_authIndicatorImage.color = NEGATIVE;
        auth.observeCurrentUser ()
            .Subscribe (user => {
                if ( user != null ) {
                    m_authIndicatorImage.color = POSITIVE;
                } else {
                    m_authIndicatorImage.color = NEGATIVE;
                }
            });
    }
}

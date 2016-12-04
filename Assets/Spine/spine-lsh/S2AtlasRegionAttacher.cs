/*▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤

★ 설계자 ☞ 이상호
★ 설계일 ☞ 2015년 03월 08일
★ E-Mail ☞ shmhlove@naver.com
★ Desc   ☞ 이 클래스는 Spine의 AtlasRegionAttacher클래스를 복사해서 수정한 버전입니다.(Attach시 원본 Offset적용)

▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤▤*/

using UnityEngine;
using System.Collections;
using Spine;

[ExecuteInEditMode]
[AddComponentMenu("Spine/S2AtlasRegionAttacher")]
public class S2AtlasRegionAttacher : MonoBehaviour
{

	[System.Serializable]
	public class SlotRegionPair {
		[SpineSlot]
		public string slot;

		[SpineAtlasRegion]
		public string region;
	}

    [HideInInspector]
    public string strAtlasAsset;
	public AtlasAsset atlasAsset;
	public SlotRegionPair[] attachments;

	Atlas atlas;

	void Awake() {
		GetComponent<SkeletonRenderer>().OnReset += Apply;
        strAtlasAsset = atlasAsset.name;
	}


	void Apply(SkeletonRenderer skeletonRenderer) {
		atlas = atlasAsset.GetAtlas();

		AtlasAttachmentLoader loader = new AtlasAttachmentLoader(atlas);

		float scaleMultiplier = skeletonRenderer.skeletonDataAsset.scale;

		var enumerator = attachments.GetEnumerator();
		while (enumerator.MoveNext()) {
			var entry = (SlotRegionPair)enumerator.Current;
			var regionAttachment = loader.NewRegionAttachment(null, entry.region, entry.region);
            regionAttachment.Width = regionAttachment.RegionOriginalWidth * scaleMultiplier;
            regionAttachment.Height = regionAttachment.RegionOriginalHeight * scaleMultiplier;
            
            var slot = skeletonRenderer.skeleton.FindSlot(entry.slot);
            RegionAttachment slotregion = slot.Attachment as RegionAttachment;
            regionAttachment.X = slotregion.X;
            regionAttachment.Y = slotregion.Y;
            regionAttachment.Rotation = slotregion.Rotation;
            
            regionAttachment.SetColor(new Color(1, 1, 1, 1));
            regionAttachment.UpdateOffset();
            
			slot.Attachment = regionAttachment;
		}
	}

    public void OnReset()
    {
        GetComponent<SkeletonRenderer>().Reset();
    }
}
